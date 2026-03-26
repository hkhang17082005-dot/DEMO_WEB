using System.Text;
using Microsoft.Extensions.Options;
using SRB_WebPortal.Configs;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Controllers.payments.services;

public interface IVNPayService
{
   void Health();
   Task<string> CreatePaymentAsync(long amount);
}

public class VNPayService : IVNPayService
{
   private readonly IOptions<SystemOptions> _systemOptions;
   private readonly IOptions<PaymentOptions> _paymentOptions;
   private readonly IHashingService _hashingService;

   public VNPayService(
      IOptions<SystemOptions> systemOptions,
      IOptions<PaymentOptions> paymentOptions,
      IHashingService hashingService
   )
   {
      _systemOptions = systemOptions;
      _paymentOptions = paymentOptions;
      _hashingService = hashingService;
   }

   public void Health()
   {
   }

   public Task<string> CreatePaymentAsync(long amount)
   {
      var returnUrl = _paymentOptions.Value.VNPayReturnUrl;

      var vnpayData = new SortedDictionary<string, string>
         {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", _paymentOptions.Value.VNPayTmnCode },
            { "vnp_Amount", (amount * 100).ToString() },
            { "vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss") },
            { "vnp_CurrCode", "VND" },
            { "vnp_IpAddr", "127.0.0.1" },
            { "vnp_Locale", "vn" },
            { "vnp_OrderInfo", "ThanhToanDonHangTest" },
            { "vnp_OrderType", "other" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_TxnRef", Guid.NewGuid().ToString("N") }
         };

      var queryString = string.Join("&",
         vnpayData
            .Where(x => !string.IsNullOrEmpty(x.Value))
            .Select(x =>
               $"{x.Key}={Uri.EscapeDataString(x.Value)}")
      );

      var secureHash = _hashingService
         .ComputeHmacSha512(queryString, _paymentOptions.Value.VNPayHashSecret);

      var paymentUrl =
         $"{_paymentOptions.Value.VNPayBaseUrl}?{queryString}&vnp_SecureHash={secureHash}";

      return Task.FromResult(paymentUrl);
   }
}
