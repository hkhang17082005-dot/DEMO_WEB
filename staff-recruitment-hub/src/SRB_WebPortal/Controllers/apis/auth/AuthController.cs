using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Extensions;

namespace SRB_WebPortal.Controllers.apis.auth;

public static class CookieKeys
{
   public const string SessionId = "SessionID";
   public const string RefreshToken = "RefreshToken";
}

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IServiceScopeFactory serviceScopeFactory) : BaseAPIController
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

   [HttpPut("refresh")]
   public async Task<IActionResult> RefreshSession()
   {
      // var deviceInfo = HttpContext.GetItem<DeviceInfo>("UserDeviceInfo");
      var deviceInfo = new DeviceInfo
      {
         OS = "Mac OS",
         Browser = "PostMan",
         Version = "11.83.5",
         Device = "MacBook Air",
         IPAdress = "127.0.0.1"
      };

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
      var result = _authService.GetMe();

      return Ok(result);
   }

   [IgnoreAntiforgeryToken]
   [HttpPost("create-profile")]
   public IActionResult CreateProfile([FromForm] CreateProfileDTO formData)
   {
      if (!ModelState.IsValid)
         return BadRequest(ModelState);

      if (HttpContext.Items["SessionLogin"] is not TokenPayload session)
         return Unauthorized();

      string userID = session.User.UserID;

      _ = Task.Run(async () =>
      {
         // Tạo một Scope mới tách biệt hoàn toàn với Request hiện tại
         using var scope = _scopeFactory.CreateScope();
         try
         {
            var scopedAuthService = scope.ServiceProvider.GetRequiredService<IAuthService>();

            await scopedAuthService.HandleCreateProfileAsync(formData, userID);
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Lỗi chạy ngầm: {ex.Message}");
         }
      });

      return Ok(new { message = "Hồ sơ đang được xử lý!" });
   }
}
