using Microsoft.Extensions.Options;
using UAParser;

using SRB_WebPortal.Utils;
using SRB_WebPortal.Configs;

namespace SRB_WebPortal.Controllers.apis.auth
{
   public class AuthMiddleware
   {
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
                  var cookieMap = new Dictionary<string, string>
                  {
                  { "SetSessionID", "SessionID" },
                  { "SetAccessToken", "AccessToken" },
                  { "SetRefreshToken", "RefreshToken" }
                  };

                  foreach (var item in cookieMap)
                  {
                     var timeCookieExpires = _jwtOptions.Value.EXP_REFRESH_TOKEN;

                     if (context.Items.TryGetValue(item.Key, out var value) && value is not null)
                     {
                        baseOptions.Expires = DateTime.UtcNow.Add(TimeUtil.ParseExpTime(timeCookieExpires));
                        context.Response.Cookies.Append(item.Value, value.ToString()!, baseOptions);
                     }
                  }
               }

               return Task.CompletedTask;
            });

            await _next(context);
         }
      }
   }
}
