namespace SRB_WebPortal.Configs
{
   public class PaymentOptions
   {
      public const string SectionName = "PaymentSettings";

      // Payment Momo
      public string MomoPartnerCode { get; set; } = null!;
      public string MomoSecretKey { get; set; } = null!;
      public string MomoAccessKey { get; set; } = null!;
      public string MomoEndpoint { get; set; } = null!;

      // Payment VNPay
      public string VNPayTmnCode { get; set; } = null!;
      public string VNPayHashSecret { get; set; } = null!;
      public string VNPayBaseUrl { get; set; } = null!;
      public string VNPayReturnUrl { get; set; } = null!;
   }
}
