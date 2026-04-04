using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SRB_ViewModel.Data;

namespace SRB_ViewModel.Entities;

public class UserProfile
{
   [Key]
   public int ProfileID { get; set; }

   public required string UserID { get; set; }

   [ForeignKey("UserID")]
   public virtual User? User { get; set; }

   [Required, MaxLength(100)]
   public string FullName { get; set; } = string.Empty;

   [Required, EmailAddress]
   public required string Email { get; set; }

   [Phone]
   public string? PhoneNumber { get; set; }

   public string? Summary { get; set; } // Giới thiệu bản thân ngắn

   public string? CVPath { get; set; }

   public string? AvatarURL { get; set; }

   public string Gender { get; set; } = GenderTypes.Unknown;

   public DateTime? Birthday { get; set; }

   public string? Address { get; set; }

   public string? JobTitle { get; set; }
   public string? ExpectedSalary { get; set; }
   public string? Experience { get; set; }
   public string? WorkType { get; set; }
   public string? Skills { get; set; }

}
