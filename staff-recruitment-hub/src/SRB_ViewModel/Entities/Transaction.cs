namespace SRB_ViewModel.Entities
{
   public class Transaction
   {
      public string Id { get; set; } = Guid.NewGuid().ToString();
      public string UserId { get; set; } = null!;
      public long Amount { get; set; }
      public string PaymentMethod { get; set; } = null!; // Momo hoặc VnPay
      public string OrderInfo { get; set; } = null!;
      public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      public virtual User User { get; set; } = null!;
   }

   public enum TransactionStatus { Pending, Success, Failed }
}