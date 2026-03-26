using SRB_WebPortal.Controllers.payments.services;

namespace SRB_WebPortal.Controllers.payments;

public class MomoPaymentGateway(IMomoService momoService) : IPaymentGateway
{
   public string Name => "Momo";

   private readonly IMomoService _momoService = momoService;

   public Task<string> CreatePaymentAsync(long amount)
   {
      return _momoService.CreatePaymentAsync(amount);
   }
}

public class VNPayPaymentGateway(IVNPayService vnPayService) : IPaymentGateway
{
   public string Name => "VnPay";
   private readonly IVNPayService _vnPayService = vnPayService;

   public Task<string> CreatePaymentAsync(long amount)
   {
      return _vnPayService.CreatePaymentAsync(amount);
   }
}
