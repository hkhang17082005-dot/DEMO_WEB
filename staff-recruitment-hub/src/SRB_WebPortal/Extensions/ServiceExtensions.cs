using SRB_WebPortal.Services;

using SRB_WebPortal.Shared;

using SRB_WebPortal.Controllers.payments;
using SRB_WebPortal.Controllers.apis.user;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Controllers.apis.business;
using SRB_WebPortal.Controllers.manager.system;

namespace SRB_WebPortal.Extensions;

public static class ServiceExtensions
{
   public static IServiceCollection AddExtensionServices(this IServiceCollection services)
   {
      services.AddScoped<IRedisService, RedisService>();

      services.AddScoped<IHashingService, HashingService>();

      services.AddScoped<IBunnyCNDService, BunnyCNDService>();

      services.AddScoped<IShareRepository, ShareRepository>();

      services.AddScoped<IJwtService, JwtService>();

      services.AddSingleton<IResendService, ResendService>();

      services.AddPaymentModule();
      services.AddAuthModule();
      services.AddUserModule();
      services.AddPostModule();
      services.AddBusinessModule();
      services.AddSystemManagerModule();

      services.AddHostedService<EmailBackgroundWorker>();

      return services;
   }
}
