namespace SRB_WebPortal.Controllers.apis.user;

public static class UserModule
{
   public static IServiceCollection AddUserModule(this IServiceCollection services)
   {
      services.AddScoped<IUserRepository, UserRepository>();

      services.AddScoped<IUserService, UserService>();

      return services;
   }
}
