using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities
{
   public class User
   {
      [Key]
      public string UserID { get; set; } = null!;

      public string Username { get; set; } = null!;
      public string? HashPassword { get; set; }

      public int RoleID { get; set; }

      public UserStatus Status { get; set; } = UserStatus.pending;

      public DateTime CreatedAt { get; set; }

      public DateTime UpdatedAt { get; set; }

      public virtual Role Role { get; set; } = null!;

      public virtual ICollection<UserRoles>? UserRoles { get; set; }

      public int? BusinessID { get; set; }

      public virtual BusinessProfile? Business { get; set; }
   }

   public enum UserStatus
   {
      pending = 0,
      inactive = 1,
      active = 2,
      deactivated = 3,
      locked = 4,
      banned = 5
   }
}
