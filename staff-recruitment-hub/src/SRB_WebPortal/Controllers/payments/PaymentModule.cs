using UAParser;

using SRB_WebPortal.Controllers.payments.services;

namespace SRB_WebPortal.Controllers.payments
{
   public static class PaymentModule
   {
      public static IServiceCollection AddPaymentModule(this IServiceCollection services)
      {
         services.AddSingleton(Parser.GetDefault());

         services.AddScoped<IMomoService, MomoService>();
         services.AddScoped<IVNPayService, VNPayService>();

         services.AddScoped<IPaymentGateway, MomoPaymentGateway>();
         services.AddScoped<IPaymentGateway, VNPayPaymentGateway>();

         services.AddScoped<IPaymentFactory, PaymentFactory>();
         return services;
      }

      public static IApplicationBuilder UsePaymentModule(this IApplicationBuilder app)
      {
         return app;
      }
   }
}
