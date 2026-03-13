namespace SRB_WebPortal.Controllers.payments
{
   public class PaymentFactory : IPaymentFactory
   {
      private readonly IEnumerable<IPaymentGateway> _gateways;

      public PaymentFactory(IEnumerable<IPaymentGateway> gateways)
      {
         _gateways = gateways;
      }

      public IPaymentGateway GetGateway(string method)
      {
         var gateway = _gateways
            .FirstOrDefault(x => x.Name.Equals(method, StringComparison.OrdinalIgnoreCase));

         if (gateway == null)
            throw new Exception("Phương thức thanh toán không hỗ trợ");

         return gateway;
      }
   }
}
