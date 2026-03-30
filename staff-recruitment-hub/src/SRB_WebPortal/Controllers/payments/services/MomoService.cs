using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SRB_WebPortal.Configs;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Controllers.payments.services;

public interface IMomoService
{
   void Health();
   Task<string> CreatePaymentAsync(long amount);
}

public class MomoService(
   IOptions<PaymentOptions> paymentOptions,
   IHttpClientFactory httpClientFactory,
   IHashingService hashingService
) : IMomoService
{
   private readonly IOptions<PaymentOptions> _paymentOptions = paymentOptions;
   private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
   private readonly IHashingService _hashingService = hashingService;

   public void Health()
   {
      return;
   }

   public async Task<string> CreatePaymentAsync(long amount)
   {
      var partnerCode = _paymentOptions.Value.MomoPartnerCode?.Trim();
      var accessKey = _paymentOptions.Value.MomoAccessKey?.Trim();
      var secretKey = _paymentOptions.Value.MomoSecretKey?.Trim();
      var endpoint = _paymentOptions.Value.MomoEndpoint?.Trim();

      if (string.IsNullOrEmpty(partnerCode) ||
          string.IsNullOrEmpty(accessKey) ||
          string.IsNullOrEmpty(secretKey) ||
          string.IsNullOrEmpty(endpoint))
      {
         throw new Exception("Momo config chưa được cấu hình đúng");
      }

      var orderId = Guid.NewGuid().ToString("N");
      var requestId = Guid.NewGuid().ToString("N");
      var orderInfo = "Thanh toán đơn hàng";

      var redirectUrl = "http://localhost:8000/payment/success";
      var ipnUrl = "http://localhost:8000/payment/ipn";

      var rawHash =
         $"accessKey={accessKey}" +
         $"&amount={amount}" +
         $"&extraData=" +
         $"&ipnUrl={ipnUrl}" +
         $"&orderId={orderId}" +
         $"&orderInfo={orderInfo}" +
         $"&partnerCode={partnerCode}" +
         $"&redirectUrl={redirectUrl}" +
         $"&requestId={requestId}" +
         $"&requestType=captureWallet";

      var signature = _hashingService.ComputeHmacSha256(rawHash, secretKey);

      var requestBody = new
      {
         partnerCode,
         accessKey,
         requestId,
         amount = amount.ToString(),
         orderId,
         orderInfo,
         redirectUrl,
         ipnUrl,
         extraData = "",
         requestType = "captureWallet",
         signature,
         lang = "vi"
      };

      var client = _httpClientFactory.CreateClient();

      var response = await client.PostAsync(
         endpoint,
         new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
      );

      var body = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
         throw new Exception($"Momo HTTP Error: {response.StatusCode} - {body}");
      }

      using var doc = JsonDocument.Parse(body);
      var root = doc.RootElement;

      if (root.TryGetProperty("payUrl", out var payUrl))
      {
         var url = payUrl.GetString();
         if (!string.IsNullOrEmpty(url))
            return url;
      }

      throw new Exception("Không tìm thấy payUrl trong response: " + body);
   }
}
