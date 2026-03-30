using SRB_WebPortal.Configs;

namespace SRB_WebPortal.Extensions;

public static class ConfigExtensions
{
   public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
   {
      services.Configure<SystemOptions>(config.GetSection(SystemOptions.SectionName));
      services.Configure<ServiceOptions>(config.GetSection(SystemOptions.SectionName));
      services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
      services.Configure<PaymentOptions>(config.GetSection(PaymentOptions.SectionName));
      services.Configure<BunnyOptions>(config.GetSection(BunnyOptions.SectionName));

      return services;
   }
}
