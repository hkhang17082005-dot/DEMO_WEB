using SRB_WebPortal.Services;

using SRB_WebPortal.Controllers.payments;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Controllers.apis.business;
using SRB_WebPortal.Controllers.manager.system;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Extensions;

public static class ServiceExtensions
{
   public static IServiceCollection AddExtensionServices(this IServiceCollection services)
   {
      services.AddScoped<IRedisService, RedisService>();
      services.AddScoped<IHashingService, HashingService>();

      services.AddScoped<IShareRepository, ShareRepository>();

      services.AddScoped<IJwtService, JwtService>();

      services.AddPaymentModule();
      services.AddAuthModule();
      services.AddPostModule();
      services.AddBusinessModule();
      services.AddSystemManagerModule();

      return services;
   }
}
