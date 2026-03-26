namespace SRB_WebPortal.Controllers.payments;

public interface IPaymentGateway
{
   string Name { get; }
   Task<string> CreatePaymentAsync(long amount);
}

public interface IPaymentFactory
{
   IPaymentGateway GetGateway(string method);
}
