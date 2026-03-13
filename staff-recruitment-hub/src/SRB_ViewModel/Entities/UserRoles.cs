namespace SRB_ViewModel.Entities
{
   public class UserRoles
   {
      public required string UserID { get; set; }

      public int RoleID { get; set; }

      public DateTime CreatedAt { get; set; }

      public DateTime UpdatedAt { get; set; }

      public virtual User User { get; set; } = null!;

      public virtual Role Role { get; set; } = null!;
   }
}
