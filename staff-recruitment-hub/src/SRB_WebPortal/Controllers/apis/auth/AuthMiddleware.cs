using UAParser;

using Microsoft.Extensions.Options;

using SRB_WebPortal.Utils;
using SRB_WebPortal.Configs;
using SRB_WebPortal.Services;
using SRB_WebPortal.Shared;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SRB_WebPortal.Controllers.apis.auth;

public class AuthMiddleware
{
   public class UserSessionMiddleware(RequestDelegate next)
   {
      private static readonly string[] RequiredClaims = ["user_id", "refresh_token", "jti"];

      public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
      {
         var accessToken = context.Request.Cookies["AccessToken"];

         if (string.IsNullOrEmpty(accessToken))
         {
            await next(context);
            return;
         }

         try
         {
            var principal = jwtService.ValidateToken(accessToken, out string failureReason);

            // Kiểm tra Token hợp lệ
            if (principal == null)
            {
               await HandleUnauthorized(context, "Token không chính xác hoặc đã hết hạn!");

               return;
            }

            var allClaims = principal.Claims.ToList();

            var roleSlugs = allClaims
               .Where(c => c.Type == ClaimTypes.Role)
               .Select(c => c.Value)
               .ToArray();

            // Kiểm tra các Claims bắt buộc
            bool hasAllRequired = RequiredClaims.All(req => allClaims.Any(c => c.Type == req));

            if (!hasAllRequired)
            {
               await HandleUnauthorized(context, "Dữ liệu Token không đầy đủ!");

               return;
            }

            // Nạp thông tin vào Items
            context.Items["SessionLogin"] = new TokenPayload
            {
               User = new UserPayload
               {
                  UserID = allClaims.FirstOrDefault(c => c.Type == "user_id")?.Value ?? string.Empty,
                  Username = allClaims.FirstOrDefault(c => c.Type == "username")?.Value ?? string.Empty,
                  Status = allClaims.FirstOrDefault(c => c.Type == "user_status")?.Value ?? string.Empty,
                  BusinessID = allClaims.FirstOrDefault(c => c.Type == "user_business")?.Value ?? string.Empty,
                  RoleSlugs = roleSlugs,
               },
               RefreshToken = allClaims.FirstOrDefault(c => c.Type == "refresh_token")?.Value ?? string.Empty,
               SessionID = allClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty,
            };
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Critical Error in Middleware: {ex.Message}");
            await HandleUnauthorized(context, "Hệ thống gặp sự cố xác thực!");
            return;
         }

         await next(context);
      }

      private static async Task HandleUnauthorized(HttpContext context, string message)
      {
         if (context.Response.HasStarted) return;

         // Xóa các Cookie lỗi để tránh loop
         context.Response.Cookies.Delete("AccessToken");
         context.Response.Cookies.Delete("SessionID");

         // Kiểm tra nếu đường dẫn bắt đầu bằng /api thì trả về JSON, ngược lại Redirect
         bool isApiRequest = context.Request.Path.StartsWithSegments("/api");

         if (isApiRequest)
         {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(BaseResponse.Unauthorized(message));
         }
         else
         {
            // Chuyển hướng về trang chủ hoặc trang Login
            context.Response.Redirect("/Login?returnUrl=" + context.Request.Path);
         }
      }
   }

   public class TokenToCookieMiddleware(
      RequestDelegate next,
      IWebHostEnvironment env,
      IOptions<JwtOptions> jwtOptions,
      Parser parser
   )
   {
      private readonly RequestDelegate _next = next;
      private readonly IWebHostEnvironment _env = env;
      private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
      private readonly Parser _uaParser = parser;
      private static readonly string[] callbackWrite = ["SessionID", "AccessToken", "RefreshToken"];

      public async Task InvokeAsync(HttpContext context)
      {
         string userAgent = context.Request.Headers.UserAgent.ToString();
         var clientInfo = _uaParser.Parse(userAgent);

         var deviceInfo = new DeviceInfo
         {
            OS = clientInfo.OS.Family,
            Browser = clientInfo.UA.Family,
            Version = clientInfo.UA.Major,
            Device = clientInfo.Device.Family,
            IPAdress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
         };

         context.Items["UserDeviceInfo"] = deviceInfo;

         context.Response.OnStarting(() =>
         {
            bool isProduction = _env.IsProduction();

            var baseOptions = new CookieOptions
            {
               HttpOnly = true,
               Secure = isProduction,
               SameSite = isProduction ? SameSiteMode.Strict : SameSiteMode.Lax,
               Path = "/"
            };

            var authCookies = callbackWrite;

            // Delete Auth Cookie
            if (context.Items.ContainsKey("DeleteAuthCookies"))
            {
               foreach (var cookieName in authCookies)
               {
                  context.Response.Cookies.Delete(cookieName, baseOptions);
               }
            }
            // Write Cookie
            else
            {
               var globalExpireTime = DateTime.UtcNow.Add(TimeUtil.ParseExpTime(_jwtOptions.Value.EXP_REFRESH_TOKEN));

               var cookieMap = new Dictionary<string, string>
               {
                  { "SetSessionID", "SessionID" },
                  { "SetAccessToken", "AccessToken" },
                  { "SetRefreshToken", "RefreshToken" }
               };

               foreach (var item in cookieMap)
               {
                  if (context.Items.TryGetValue(item.Key, out var value) && value is not null)
                  {
                     var currentOptions = new CookieOptions
                     {
                        HttpOnly = baseOptions.HttpOnly,
                        Secure = baseOptions.Secure,
                        SameSite = baseOptions.SameSite,
                        Path = baseOptions.Path,
                        Expires = globalExpireTime
                     };

                     context.Response.Cookies.Append(item.Value, value.ToString()!, currentOptions);
                  }
               }
            }

            return Task.CompletedTask;
         });

         await _next(context);
      }
   }
}
