using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.apis.business;

public static class BusinessModule
{
   public static IServiceCollection AddBusinessModule(this IServiceCollection services)
   {
      services.AddScoped<IBusinessRepository, BusinessRepository>();

      services.AddScoped<BusinessService>();

      return services;
   }
}
