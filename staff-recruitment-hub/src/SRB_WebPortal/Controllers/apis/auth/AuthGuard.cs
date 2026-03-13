using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Services;
using SRB_WebPortal.Shared;

public class AuthGuard : ActionFilterAttribute
{
   public string[] Roles { get; set; } = [];

   public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
   {
      return;

      var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
      var redisService = context.HttpContext.RequestServices.GetRequiredService<IRedisService>();

      var sessionID = context.HttpContext.Request.Cookies["SessionID"];
      var accessToken = context.HttpContext.Request.Cookies["AccessToken"];
      var refreshToken = context.HttpContext.Request.Cookies["RefreshToken"];

      if (string.IsNullOrEmpty(sessionID) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
      {
         context.Result = new JsonResult(BaseResponse.Unauthorized("Đăng nhập trước khi sử dụng chức năng này!"))
         {
            StatusCode = StatusCodes.Status401Unauthorized
         };
         return;
      }

      var tokenPayload = jwtService.ValidateToken(accessToken!, out string failureReason);
      if (tokenPayload is null)
      {
         if (failureReason == BackendSignals.TAMPERED_TOKEN)
         {
            // Xử lý chống hacker
         }

         context.Result = new JsonResult(BaseResponse.Unauthorized("Token không chính xác hoặc đã Hết hạn!", failureReason))
         {
            StatusCode = StatusCodes.Status401Unauthorized
         };
         return;
      }

      var userId = tokenPayload.FindFirst("user_id")?.Value;
      var sessionId = tokenPayload.FindFirst("jti")?.Value;
      var refreshTokenHash = tokenPayload.FindFirst("refresh_token")?.Value;

      if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(refreshTokenHash))
      {
         context.Result = new JsonResult(BaseResponse.BadRequest("Không tìm thấy thông tin Cần thiết của Token!"))
         {
            StatusCode = StatusCodes.Status400BadRequest
         };
         return;
      }

      var SessionInfoKey = RedisCacheKeys.SessionInfo(userId, sessionId);
      var sessionInfoCache = await redisService.GetAsync<SessionInfo>(SessionInfoKey);
      if (
         sessionInfoCache is null ||
         sessionID != sessionId
      )
      {
         var response = BaseResponse.Unauthorized("Bạn không thể thực hiện thao tác này!", BackendSignals.SESSION_NOT_FOUND);
         context.Result = new JsonResult(response)
         {
            StatusCode = StatusCodes.Status401Unauthorized
         };
         return;
      }

      var userRole = tokenPayload?.FindFirst("user_role")?.Value?.ToUpper();
      if (Roles?.Length > 0 && !Roles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
      {
         context.Result = new JsonResult(BaseResponse.Forbidden("Bạn không có quyền truy cập!"))
         {
            StatusCode = StatusCodes.Status403Forbidden
         };
         return;
      }

      // Gắn thông tin vào Request
      context.HttpContext.Items["SessionInfo"] = sessionInfoCache;

      await next();
      // Handle Response
   }
}
