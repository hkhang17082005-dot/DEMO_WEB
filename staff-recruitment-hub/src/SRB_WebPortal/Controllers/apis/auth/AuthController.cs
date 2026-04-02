using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Consts;
using SRB_WebPortal.Extensions;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.auth;

public static class CookieKeys
{
   public const string SessionId = "SessionID";
   public const string RefreshToken = "RefreshToken";
}

[ApiController]
[Route("api/[controller]")]
public class AuthController(
   IAuthService authService,
   IServiceScopeFactory serviceScopeFactory
) : BaseAPIController
{
   private readonly IAuthService _authService = authService;
   private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

   [HttpGet("health")]
   [AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER })]
   public async Task<IActionResult> Health()
   {
      return Ok(await _authService.Health());
   }

   [IsPublic]
   [HttpPost("login")]
   public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
   {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var deviceInfo = HttpContext.GetItem<DeviceInfo>("UserDeviceInfo");

      var result = await _authService.Login(model, deviceInfo);

      if (!result.IsSuccess)
      {
         return StatusCode(result.StatusCode, result);
      }

      return Ok(result);
   }

   [IsPublic]
   [HttpDelete("logout")]
   public async Task<IActionResult> Logout()
   {
      var sessionId = Request.Cookies[CookieKeys.SessionId];
      var refreshToken = Request.Cookies[CookieKeys.RefreshToken];

      var result = await _authService.Logout(sessionId, refreshToken);

      return Ok(result);
   }

   [IsPublic]
   [HttpPost("register")]
   public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
   {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var deviceInfo = HttpContext.GetItem<DeviceInfo>("UserDeviceInfo");

      var result = await _authService.Register(model, deviceInfo);

      if (!result.IsSuccess)
      {
         return BadRequest(result);
      }

      return Ok(result);
   }

   [IsPublic]
   [HttpPut("refresh")]
   public async Task<IActionResult> RefreshSession()
   {
      var deviceInfo = HttpContext.GetItem<DeviceInfo>("UserDeviceInfo");

      var sessionId = Request.Cookies[CookieKeys.SessionId];
      var refreshToken = Request.Cookies[CookieKeys.RefreshToken];

      var result = await _authService.RefreshSession(deviceInfo, sessionId, refreshToken);

      if (!result.IsSuccess)
      {
         return BadRequest(result);
      }

      return Ok(result);
   }

   [HttpGet("me")]
   public async Task<IActionResult> GetMe()
   {
      if (string.IsNullOrEmpty(CurrentUserID)) return Unauthorized("Không tìm thấy Thông tin cần thiết!");

      var result = await _authService.GetMe(CurrentUserID);

      return HandleResult(result);
   }

   [HttpPost("create-profile")]
   public async Task<BaseResponse> CreateProfile([FromForm] CreateProfileDTO formData)
   {
      if (!ModelState.IsValid || string.IsNullOrEmpty(CurrentUserID))
         return BaseResponse.BadRequest("Dữ liệu không hợp lệ");

      var userID = CurrentUserID;

      // Copy stream của file ra bộ nhớ trước khi Request kết thúc
      Stream? avatarStream = null;
      string? avatarName = null;
      if (formData.AvatarFile != null)
      {
         avatarStream = new MemoryStream();
         await formData.AvatarFile.CopyToAsync(avatarStream);
         avatarStream.Position = 0;
         avatarName = formData.AvatarFile.FileName;
      }

      Stream? cvStream = null;
      string? cvName = null;
      if (formData.CVFile != null)
      {
         cvStream = new MemoryStream();
         await formData.CVFile.CopyToAsync(cvStream);
         cvStream.Position = 0;

         cvName = formData.CVFile.FileName;
      }

      // Chạy ngầm an toàn
      _ = Task.Run(async () =>
      {
         using var scope = _scopeFactory.CreateScope();
         try
         {
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            await authService.HandleCreateProfileAsync(formData, userID, avatarStream, avatarName, cvStream, cvName);
         }
         catch (Exception ex)
         {
            Console.WriteLine($"[Background Error]: {ex.Message}");
         }
         finally
         {
            avatarStream?.Dispose();
            cvStream?.Dispose();
         }
      });

      return BaseResponse.Success("Hồ sơ của bạn đang được xử lý!");
   }
}
