using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities;

public class User
{
   [Key]
   public string UserID { get; set; } = null!;

   public string Username { get; set; } = null!;

   public string? HashPassword { get; set; }

   public UserStatus Status { get; set; } = UserStatus.pending;

   public DateTime CreatedAt { get; set; }

   public DateTime UpdatedAt { get; set; }

   public virtual ICollection<UserRoles> UserRoles { get; set; } = [];

   [ForeignKey("UserBusinessID")]
   public string? BusinessID { get; set; }

   public virtual ICollection<SavedJob>? SavedJobs { get; set; }

   public virtual Business? Business { get; set; }

   public virtual UserProfile? UserProfile { get; set; }
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
