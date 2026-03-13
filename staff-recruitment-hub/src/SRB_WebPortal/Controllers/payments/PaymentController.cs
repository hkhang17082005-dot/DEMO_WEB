using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.payments
{
   [Route("payment")]
   public class PaymentController(IPaymentFactory paymentFactory) : Controller
   {
      private readonly IPaymentFactory _paymentFactory = paymentFactory;

      public IActionResult Index()
      {
         return View();
      }

      [HttpGet("success")]
      public IActionResult Success()
      {
         return Content("Thanh toán thành công");
      }

      [HttpPost("ipn")]
      public IActionResult IPN()
      {
         return Ok();
      }

      [HttpPost("create")]
      public async Task<IActionResult> CreatePayment(long? Amount, long? CustomAmount, string Method)
      {
         long finalAmount = CustomAmount.HasValue && CustomAmount > 0
            ? CustomAmount.Value
            : Amount ?? 0;

         if (finalAmount <= 0)
            return BadRequest("Số tiền không hợp lệ");

         var gateway = _paymentFactory.GetGateway(Method);

         var paymentUrl = await gateway.CreatePaymentAsync(finalAmount);

         return Redirect(paymentUrl);
      }

      [HttpGet("vnpay-return")]
      public IActionResult VNPayReturn()
      {
         // var query = Request.Query;

         // var vnp_SecureHash = query["vnp_SecureHash"];
         // var hashSecret = _config["VNPay:HashSecret"];

         // var data = new SortedDictionary<string, string>();

         // foreach (var key in query.Keys)
         // {
         //    if (key.StartsWith("vnp_") && key != "vnp_SecureHash")
         //    {
         //       data.Add(key, query[key]!);
         //    }
         // }

         // var rawData = BuildQueryString(data);
         // var signData = HmacSHA512(hashSecret, rawData);

         // if (signData == vnp_SecureHash)
         // {
         //    if (query["vnp_ResponseCode"] == "00")
         //       return Content("Thanh toán VNPay thành công");
         //    else
         //       return Content("Thanh toán thất bại");
         // }

         // return Content("Sai chữ ký VNPay");
         return Content("Thanh toán VNPay thành công");
      }
   }
}
