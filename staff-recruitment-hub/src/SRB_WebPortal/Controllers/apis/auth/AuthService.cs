using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
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
   Task<List<User>> Health();
   Task<BaseResponse<AuthResponse>> Login(LoginModelDTO model, DeviceInfo? deviceInfo);
   Task<BaseResponse> Logout(string? sessionId, string? refreshToken);
   Task<BaseResponse> Register(RegisterModelDTO model, DeviceInfo? deviceInfo);
   Task<BaseResponse<AuthResponse>> RefreshSession(DeviceInfo? deviceInfo, string? sessionId, string? refreshToken);
   Task<BaseResponse> GetMe();
   Task HandleCreateProfileAsync(CreateProfileDTO formData, string UserID);
}

public class AuthService(
   IRedisService redisService,
   IJwtService jwtService,
   DatabaseContext context,
   IHashingService hashingService,
   IAuthRepository authRepository,
   IHttpContextAccessor httpContextAccessor,
   IOptions<JwtOptions> jwtOptions,
   TokenFactory tokenFactory,
   Cloudinary cloudinary
   ) : IAuthService
{
   private readonly IRedisService _redisService = redisService;
   private readonly IJwtService _jwtService = jwtService;
   private readonly DatabaseContext _context = context;
   private readonly IHashingService _hashingService = hashingService;
   private readonly IAuthRepository _authRepository = authRepository;
   private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
   private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
   private readonly TokenFactory _tokenFactory = tokenFactory;
   private readonly Cloudinary _cloudinary = cloudinary;

   public async Task<List<User>> Health()
   {
      return await _context.Users.ToListAsync();
   }

   public async Task<BaseResponse<AuthResponse>> Login(LoginModelDTO model, DeviceInfo? deviceInfo)
   {
      var userLogin = await _authRepository.GetUserByUsername(model.Username);
      if (userLogin is null || !_hashingService.VerifyHashValue(model.Password, userLogin.HashPassword))
      {
         return BaseResponse<AuthResponse>.Failure(
            "Tài khoản hoặc Mật khẩu không chính xác",
            HttpStatusCode.BadRequest
         );
      }
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

      var roleSlugDefault = Roles.CANDIDATE;
      string roleKey = RedisCacheKeys.RoleKey(roleSlugDefault);
      var cacheRoleID = await _redisService.GetAsync<int?>(roleKey);

      int roleDefaultID;
      if (cacheRoleID == null)
      {
         roleDefaultID = await _authRepository.GetRoleIDBySlug(roleSlugDefault);
         if (roleDefaultID == -1)
         {
            return BaseResponse.BadRequest("Xảy ra lỗi khi tạo tài khoản mới!");
         }
         await _redisService.SetAsync(roleKey, roleDefaultID);
      }
      else
      {
         roleDefaultID = cacheRoleID.Value;
      }

      var userID = Guid.CreateVersion7().ToString();
      string hashedPassword = _hashingService.HashValue(model.Password);

      await _authRepository.CreateNewUser(userID, model.Username, hashedPassword, roleDefaultID);

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

      var roleSlugs = userRefresh.UserRoles?
         .Select(ur => ur.Role.RoleSlug)
         .Where(slug => !string.IsNullOrEmpty(slug))
         .ToArray() ?? [];

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
         Status = userRefresh.Status.ToString()
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

   public async Task<BaseResponse> GetMe()
   {
      return BaseResponse.Success("API Get Me Successful");
   }

   public async Task HandleCreateProfileAsync(CreateProfileDTO formData, string UserID)
   {
      string? avatarURL = null;
      string? cvURL = null;

      try
      {
         if (formData.AvatarFile != null && formData.AvatarFile.Length > 0)
         {
            var uploadParams = new ImageUploadParams()
            {
               File = new FileDescription(formData.AvatarFile.FileName, formData.AvatarFile.OpenReadStream()),
               Folder = CloudinaryKey.FOLDER_PROFILE_AVATAR,
               Transformation = new Transformation().Width(500).Height(500).Crop("fill") // Auto Resize
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            avatarURL = uploadResult.SecureUrl.ToString();
         }

         // Xử lý Upload CV (Dạng Raw file/PDF)
         if (formData.CVFile != null && formData.CVFile.Length > 0)
         {
            var uploadParams = new RawUploadParams()
            {
               File = new FileDescription(formData.CVFile.FileName, formData.CVFile.OpenReadStream()),
               Folder = CloudinaryKey.FOLDER_PROFILE_CV
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            cvURL = uploadResult.SecureUrl.ToString();
         }

         await _authRepository.SaveProfile(formData, UserID, avatarURL, cvURL);

         // Giả lập
         await Task.Delay(2000);
      }
      catch (Exception ex)
      {
         // Log lỗi upload
         // _logger.LogError($"Lỗi khi xử lý Profile ngầm: {ex.Message}");
         Console.WriteLine("NQHxLog: " + ex.Message);
      }
   }
}
