using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

using SRB_ViewModel.Data;
using SRB_WebPortal.Utils;
using SRB_WebPortal.Consts;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Configs;
using SRB_WebPortal.Services;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.auth;

public interface IAuthService
{
   Task<BaseResponse> Health();
   Task<BaseResponse<AuthResponse>> Login(LoginModelDTO model, DeviceInfo? deviceInfo);
   Task<BaseResponse> Logout(string? sessionId, string? refreshToken);
   Task<BaseResponse> Register(RegisterModelDTO model, DeviceInfo? deviceInfo);
   Task<BaseResponse<AuthResponse>> RefreshSession(DeviceInfo? deviceInfo, string? sessionId, string? refreshToken);
   Task<BaseResponse<UserMeResponse>> GetMe(string userID);
   Task HandleCreateProfileAsync(CreateProfileDTO formData, string userID, Stream? avatarStream, string? avatarName, Stream? cvStream, string? cvName);
}

public class AuthService(
   IRedisService redisService,
   IJwtService jwtService,
   IHashingService hashingService,
   IAuthRepository authRepository,
   IHttpContextAccessor httpContextAccessor,
   IOptions<JwtOptions> jwtOptions,
   TokenFactory tokenFactory,
   Cloudinary cloudinary,
   IShareRepository shareRepository,
   IBunnyCNDService bunnyCNDService,
   IResendService resendService
   ) : IAuthService
{
   private readonly IRedisService _redisService = redisService;
   private readonly IJwtService _jwtService = jwtService;
   private readonly IHashingService _hashingService = hashingService;
   private readonly IAuthRepository _authRepository = authRepository;
   private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
   private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
   private readonly TokenFactory _tokenFactory = tokenFactory;
   private readonly Cloudinary _cloudinary = cloudinary;
   private readonly IShareRepository _shareRepository = shareRepository;
   private readonly IBunnyCNDService _bunnyCNDService = bunnyCNDService;
   private readonly IResendService _mailService = resendService;

   public async Task<BaseResponse> Health()
   {
      await _mailService.SendMailAsync("hkhang17082005@gmail.com", "Chào mừng", "Cảm ơn bạn!");

      return BaseResponse.Success("API Auth is Running!");
   }

   public async Task<BaseResponse<AuthResponse>> Login(LoginModelDTO model, DeviceInfo? deviceInfo)
   {
      var userLogin = await _authRepository.GetUserByUsername(model.Username);

      // Kiểm tra nếu user không tồn tại hoặc mật khẩu không khớp
      if (userLogin is null || !_hashingService.VerifyHashValue(model.Password, userLogin.HashPassword))
      {
         return BaseResponse<AuthResponse>.Failure(
            "Tài khoản hoặc Mật khẩu không chính xác",
            HttpStatusCode.BadRequest
         );
      }

      // Kiểm tra trạng thái người dùng
      if (userLogin.Status == UserStatus.locked)
      {
         return BaseResponse<AuthResponse>.Failure(
            "Tai khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên để biết thêm chi tiết.",
            HttpStatusCode.BadRequest
         );
      }

      //Cập nhật trạng thái khi đăng nhập
      await _authRepository.UpdateUserStatus(userLogin.UserID, "active");

      var roleSlugs = userLogin.UserRoles.Select(ur => ur.Role.RoleSlug).ToArray();
      if (roleSlugs is null)
      {
         return BaseResponse<AuthResponse>.Failure(
            "Xảy ra lỗi khi Đăng nhập",
            HttpStatusCode.InternalServerError
         );
      }

      var sessionId = Guid.CreateVersion7().ToString();
      var userPayload = new UserPayload
      {
         UserID = userLogin.UserID,
         Username = userLogin.Username,
         RoleSlugs = roleSlugs,
         Status = userLogin.Status.ToString(),
         BusinessID = userLogin.BusinessID
      };

      await SetNewSession(sessionId, userPayload, deviceInfo);

      var response = new AuthResponse
      {
         User = userPayload,
         CreatedAt = DateTime.UtcNow,
      };

      return BaseResponse<AuthResponse>.Success(response, "Đăng nhập thành công");
   }

   public async Task<BaseResponse> Logout(string? sessionId, string? refreshToken)
   {
      if (sessionId is not null)
      {
         string RefreshTokenKey = RedisCacheKeys.RefreshToken(sessionId);
         var refreshInfo = await _redisService.GetAsync<SessionCacheInfo>(RefreshTokenKey);
         if (refreshInfo is not null)
         {
            // Cập nhật trạng thái người dùng thành "inactive" khi đăng xuất
            await _authRepository.UpdateUserStatus(refreshInfo.UserID, "inactive");

            await _redisService.RemoveAsync(RefreshTokenKey);

            var SessionInfoKey = RedisCacheKeys.SessionInfo(refreshInfo.UserID, sessionId);
            await _redisService.RemoveAsync(SessionInfoKey);

            _httpContextAccessor.HttpContext?.Items.Add("DeleteAuthCookies", true);
         }
      }
      return BaseResponse.Success("Logout Success", HttpStatusCode.OK);
   }

   public async Task<BaseResponse> Register(RegisterModelDTO model, DeviceInfo? deviceInfo)
   {
      if (await _authRepository.ExistingUsername(model.Username))
      {
         return BaseResponse.BadRequest("Đăng ký thất bại! Tên đăng nhập đã tồn tại");
      }

      string roleSlugDefault = Roles.CANDIDATE;
      var roleDefaultID = await _shareRepository.GetRoleIDBySlug(roleSlugDefault);
      if (roleDefaultID is null || roleDefaultID <= 0)
      {
         Console.WriteLine("NQHxLog: Không tìm thấy Role ID");

         return BaseResponse.InternalServerError("Hệ thống xảy ra lỗi! Vui lòng thử lại sau!");
      }

      var userID = Guid.CreateVersion7().ToString();
      string hashedPassword = _hashingService.HashValue(model.Password);

      await _authRepository.CreateNewUser(userID, model.Username, hashedPassword, roleDefaultID.Value);

      var sessionId = Guid.CreateVersion7().ToString();

      string[] roleSlugs = [roleSlugDefault];
      var userPayload = new UserPayload
      {
         UserID = userID,
         Username = model.Username,
         RoleSlugs = roleSlugs,
         Status = "pending"
      };

      await SetNewSession(sessionId, userPayload, deviceInfo);

      return BaseResponse.Success("Đăng ký thành công");
   }

   private async Task<(TokenPayload payload, string rawRefreshToken)> CreateSession(
      UserPayload userPayload,
      string? sessionId = null,
      DeviceInfo? deviceInfo = null
   )
   {
      if (string.IsNullOrWhiteSpace(sessionId))
      {
         sessionId = _tokenFactory.CreateSessionId();
      }
      var (rawRefresh, hashRefresh) = _tokenFactory.CreateRefreshToken();

      var tokenPayload = new TokenPayload
      {
         User = userPayload,
         SessionID = sessionId,
         RefreshToken = hashRefresh,
         CreatedAt = DateTime.UtcNow
      };

      deviceInfo ??= new DeviceInfo
      {
         OS = "Unknown",
         Browser = "Unknown",
         Version = "Unknown",
         Device = "Unknown",
         IPAdress = "Unknown"
      };

      var sessionInfo = new SessionInfo
      {
         Payload = tokenPayload,
         DeviceInfo = deviceInfo,
         LastAccessed = DateTime.UtcNow
      };

      await SetCacheAuthentication(tokenPayload.User.UserID, sessionId, hashRefresh, sessionInfo);

      return (tokenPayload, rawRefresh);
   }

   public async Task<BaseResponse<AuthResponse>> RefreshSession(DeviceInfo? deviceInfo, string? sessionId, string? refreshToken)
   {
      if (deviceInfo is null || string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(refreshToken))
      {
         return BaseResponse<AuthResponse>.Failure(
            "Cập nhật thất bại Session thông tin không chính xác",
            HttpStatusCode.BadRequest
         );
      }

      string RefreshTokenKey = RedisCacheKeys.RefreshToken(sessionId);
      var refreshInfo = await _redisService.GetAsync<SessionCacheInfo>(RefreshTokenKey);
      if (refreshInfo is null || !_hashingService.VerifyHashValue(refreshToken, refreshInfo.RefreshTokenHash))
      {
         return BaseResponse<AuthResponse>.Failure(
            "Cập nhật thất bại Session Token không chính xác",
            HttpStatusCode.BadRequest
         );
      }

      var SessionInfoKey = RedisCacheKeys.SessionInfo(refreshInfo.UserID, sessionId);
      var sessionInfo = await _redisService.GetAsync<SessionInfo>(SessionInfoKey);
      if (sessionInfo is null)
      {
         return BaseResponse<AuthResponse>.Failure(
            "Bạn không thể thực hiện thao tác này",
            HttpStatusCode.Unauthorized,
            BackendSignals.SESSION_NOT_FOUND
         );
      }

      var userRefresh = await _authRepository.GetUserByUserID(sessionInfo.Payload.User.UserID);
      if (userRefresh is null)
      {
         return BaseResponse<AuthResponse>.Failure(
            "Xảy ra lỗi khi lấy thông tin người dùng",
            HttpStatusCode.BadRequest
         );
      }


      var roleSlugs = await _authRepository.GetUserRoleSlugs(userRefresh.UserID);

      if (roleSlugs is null)
      {
         return BaseResponse<AuthResponse>.Failure(
            "Xảy ra lỗi khi Đăng nhập",
            HttpStatusCode.InternalServerError
         );
      }

      var userPayload = new UserPayload
      {
         UserID = userRefresh.UserID,
         Username = userRefresh.Username,
         RoleSlugs = roleSlugs,
         Status = userRefresh.Status.ToString(),
         BusinessID = userRefresh.BusinessID
      };

      var (rawRefresh, hashRefresh) = _tokenFactory.CreateRefreshToken();

      var tokenPayload = new TokenPayload
      {
         User = userPayload,
         SessionID = sessionId,
         RefreshToken = hashRefresh,
         CreatedAt = DateTime.UtcNow
      };

      await SetCacheAuthentication(tokenPayload.User.UserID, sessionId, hashRefresh, sessionInfo);

      var expConfig = _jwtOptions.Value.EXP_ACCESS_TOKEN;
      var expiresAccessToken = DateTime.UtcNow.Add(TimeUtil.ParseExpTime(expConfig));

      var AccessToken = _jwtService.GenerateToken(tokenPayload, expiresAccessToken);

      _httpContextAccessor.HttpContext?.Items.Add("SetSessionID", sessionId);
      _httpContextAccessor.HttpContext?.Items.Add("SetRefreshToken", rawRefresh);
      _httpContextAccessor.HttpContext?.Items.Add("SetAccessToken", AccessToken);

      var response = new AuthResponse
      {
         User = userPayload,
         CreatedAt = DateTime.UtcNow,
      };

      return BaseResponse<AuthResponse>.Success(response, "Refresh Session Success");
   }

   private async Task SetNewSession(
      string sessionID, UserPayload userPayload, DeviceInfo? deviceInfo)
   {
      var (tokenPayload, refreshToken) = await CreateSession(userPayload, sessionID, deviceInfo);

      var expConfigExpAccessToken = _jwtOptions.Value.EXP_ACCESS_TOKEN;
      var expiresAccessToken = DateTime.UtcNow.Add(TimeUtil.ParseExpTime(expConfigExpAccessToken));

      var AccessToken = _jwtService.GenerateToken(tokenPayload, expiresAccessToken);

      _httpContextAccessor.HttpContext?.Items.Add("SetSessionID", tokenPayload.SessionID);
      _httpContextAccessor.HttpContext?.Items.Add("SetRefreshToken", refreshToken);
      _httpContextAccessor.HttpContext?.Items.Add("SetAccessToken", AccessToken);
   }

   private async Task SetCacheAuthentication(
      string userId,
      string sessionId,
      string refreshTokenHash,
      SessionInfo sessionInfo
   )
   {
      var expConfig = _jwtOptions.Value.EXP_REFRESH_TOKEN;
      var expiresRefreshToken = TimeUtil.ParseExpTime(expConfig);

      string SessionInfoKey = RedisCacheKeys.SessionInfo(userId, sessionId);
      await _redisService.SetAsync(SessionInfoKey, sessionInfo, expiresRefreshToken);

      string RefreshTokenKey = RedisCacheKeys.RefreshToken(sessionId);
      await _redisService.SetAsync(
         RefreshTokenKey,
         new SessionCacheInfo { UserID = userId, RefreshTokenHash = refreshTokenHash },
         expiresRefreshToken
      );
   }

   public async Task<BaseResponse<UserMeResponse>> GetMe(string userID)
   {
      var userMe = await _authRepository.GetMe(userID);

      if (userMe == null)
      {
         return BaseResponse<UserMeResponse>.Failure(
            "Người dùng không tồn tại hoặc dữ liệu không hợp lệ!",
            HttpStatusCode.BadRequest
         );
      }

      return BaseResponse<UserMeResponse>.Success(userMe, "Lấy thông tin người dùng thành công");
   }

   public async Task HandleCreateProfileAsync(CreateProfileDTO formData, string userID, Stream? avatarStream, string? avatarName, Stream? cvStream, string? cvName)
   {
      string? avatarURL = null;
      string? cvURL = null;

      // Upload Avatar
      if (avatarStream != null)
      {
         var uploadParams = new ImageUploadParams()
         {
            File = new FileDescription(avatarName, avatarStream),
            Folder = CloudCNDKey.FOLDER_PROFILE_AVATAR,
            Transformation = new Transformation().Width(500).Height(500).Crop("fill")
         };
         var result = await _cloudinary.UploadAsync(uploadParams);

         avatarURL = result.SecureUrl.ToString();
      }

      if (cvStream != null)
      {
         var result = await _bunnyCNDService.UploadToBunnyRunBackground(cvStream, cvName!, CloudCNDKey.FOLDER_PROFILE_CV);

         if (result != null && result.IsSuccess) cvURL = result.Data?.ToString();
      }

      await _authRepository.SaveProfile(formData, userID, avatarURL, cvURL);
   }

}
