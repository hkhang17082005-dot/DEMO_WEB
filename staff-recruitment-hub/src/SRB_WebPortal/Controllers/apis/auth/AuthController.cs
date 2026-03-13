using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Extensions;

namespace SRB_WebPortal.Controllers.apis.auth
{
   public static class CookieKeys
   {
      public const string SessionId = "SessionID";
      public const string RefreshToken = "RefreshToken";
   }

   [Route("api/[controller]")]
   [ApiController]
   public class AuthController(IAuthService authService) : ControllerBase
   {
      private readonly IAuthService _authService = authService;

      [HttpGet]
      [AuthGuard(Roles = new[] { "admin", "system_manager" })]
      public async Task<IActionResult> Health()
      {
         return Ok(await _authService.Health());
      }

      [HttpPost("login")]
      public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
      {
         var deviceInfo = HttpContext.GetItem<DeviceInfo>("UserDeviceInfo");
         var result = await _authService.Login(model, deviceInfo);

         if (!result.IsSuccess)
         {
            return StatusCode(result.StatusCode, result);
         }

         return Ok(result);
      }

      [HttpDelete("logout")]
      public async Task<IActionResult> Logout()
      {
         var sessionId = Request.Cookies[CookieKeys.SessionId];
         var refreshToken = Request.Cookies[CookieKeys.RefreshToken];

         var result = await _authService.Logout(sessionId, refreshToken);

         return Ok(result);
      }

      [HttpPost("register")]
      public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
      {
         var result = await _authService.Register(model);

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

      [AuthGuard]
      [HttpGet("me")]
      public async Task<IActionResult> GetMe()
      {
         var result = _authService.GetMe();

         return Ok(result);
      }
   }
}
