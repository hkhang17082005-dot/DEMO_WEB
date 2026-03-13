using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Controllers.Apis.Business;
using SRB_WebPortal.Controllers.payments;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Extensions
{
   public static class ServiceExtensions
   {
      public static IServiceCollection AddBusinessServices(this IServiceCollection services)
      {
         services.AddScoped<IRedisService, RedisService>();
         services.AddScoped<IHashingService, HashingService>();

         services.AddScoped<IJwtService, JwtService>();
         services.AddScoped<JwtGuard>();

         services.AddPaymentModule();
         services.AddAuthModule();
         services.AddPostModule();

         services.AddScoped<BusinessRepository>();
         services.AddScoped<BusinessService>();

         return services;
      }
   }
}
