using UAParser;

using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.auth;

public static class AuthModule
{
   public static IServiceCollection AddAuthModule(this IServiceCollection services)
   {
      services.AddSingleton(Parser.GetDefault());

      services.AddScoped<IAuthRepository, AuthRepository>();

      services.AddScoped<TokenFactory>();

      services.AddScoped<IAuthService, AuthService>();

      return services;
   }

   public static IApplicationBuilder UseAuthModule(this IApplicationBuilder app)
   {
      app.UseMiddleware<AuthMiddleware.UserSessionMiddleware>();

      app.UseMiddleware<AuthMiddleware.TokenToCookieMiddleware>();

      return app;
   }
}
