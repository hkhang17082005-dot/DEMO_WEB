namespace SRB_WebPortal.Controllers.manager.system;

public static class ManagerSystemModule
{
   public static IServiceCollection AddSystemManagerModule(this IServiceCollection services)
   {
      services.AddScoped<ISystemRepository, SystemRepository>();

      services.AddScoped<ISystemService, SystemService>();

      return services;
   }
}
