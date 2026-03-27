using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using SRB_WebPortal.Consts;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Controllers.apis.auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsPublicAttribute : Attribute
{
}

public class AuthGuardAttribute : ActionFilterAttribute
{
   public string[] Roles { get; set; } = [];

   public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
   {
      // Kiểm tra Attribute IsPublic
      var endpoint = context.HttpContext.GetEndpoint();
      if (endpoint?.Metadata.GetMetadata<IsPublicAttribute>() != null)
      {
         await next();
         return;
      }

      var httpContext = context.HttpContext;
      var requestPath = httpContext.Request.Path.Value ?? string.Empty;

      bool isApiRequest = requestPath.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
      bool isManagerBusinessRequest = requestPath.StartsWith("/manager/business", StringComparison.OrdinalIgnoreCase);

      if (!isApiRequest && !isManagerBusinessRequest)
      {
         await next();

         return;
      }

      var rawData = httpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO];
      if (rawData is not TokenPayload tokenPayload)
      {
         context.Result = isApiRequest
            ? new JsonResult(BaseResponse.Unauthorized("Vui lòng đăng nhập!")) { StatusCode = 401 }
            : new RedirectResult("/Login?returnUrl=" + requestPath);

         return;
      }

      if (isManagerBusinessRequest && string.IsNullOrEmpty(tokenPayload.User.BusinessID))
      {
         context.Result = new RedirectResult("/Business/RegisterBusiness");

         return;
      }

      var redisService = httpContext.RequestServices.GetRequiredService<IRedisService>();

      var cookieSessionID = httpContext.Request.Cookies["SessionID"];
      if (string.IsNullOrEmpty(cookieSessionID) || cookieSessionID != tokenPayload.SessionID)
      {
         context.Result = new JsonResult(BaseResponse.Unauthorized("Phiên đăng nhập không hợp lệ!"))
         {
            StatusCode = StatusCodes.Status401Unauthorized
         };

         return;
      }

      // Kiểm tra có yêu cầu phân quyền
      if (Roles.Length > 0)
      {
         var userRoles = tokenPayload.User.RoleSlugs;

         // Kiểm tra xem User có sở hữu Role nào nằm trong danh sách cho phép không
         bool hasAccess = userRoles.Any(userRole => Roles.Contains(userRole, StringComparer.OrdinalIgnoreCase));

         if (!hasAccess)
         {
            var shareRepository = context.HttpContext.RequestServices.GetRequiredService<IShareRepository>();
            bool isHasRoleInDatabase = await shareRepository.IsHasRole(tokenPayload.User.UserID, Roles);

            if (!isHasRoleInDatabase)
            {
               context.Result = new JsonResult(BaseResponse.Forbidden("Bạn không có quyền truy cập!"))
               {
                  StatusCode = StatusCodes.Status403Forbidden
               };

               return;
            }
         }
      }

      var sessionKey = RedisCacheKeys.SessionInfo(tokenPayload.User.UserID, tokenPayload.SessionID);
      var sessionInfoCache = await redisService.GetAsync<SessionInfo>(sessionKey);

      if (sessionInfoCache is null)
      {
         context.Result = new JsonResult(BaseResponse.Unauthorized("Phiên làm việc đã hết hạn!", BackendSignals.SESSION_NOT_FOUND))
         {
            StatusCode = StatusCodes.Status401Unauthorized
         };

         return;
      }

      httpContext.Items[ServerKey.CONTEXT_ITEM_SESSION_LOGIN] = sessionInfoCache;

      await next();
   }
}
