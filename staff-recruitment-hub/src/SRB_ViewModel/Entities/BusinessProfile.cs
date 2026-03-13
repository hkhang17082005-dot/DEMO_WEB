namespace SRB_ViewModel.Entities
{
   public class BusinessProfile
   {
      public int Id { get; set; }
      public string CompanyName { get; set; } = null!;
      public string? LogoUrl { get; set; }
      public string? Website { get; set; }
      public string? Description { get; set; }
      // Liên kết 1 công ty có nhiều nhân viên (Users)
      public virtual ICollection<User>? Employees { get; set; }
   }
}