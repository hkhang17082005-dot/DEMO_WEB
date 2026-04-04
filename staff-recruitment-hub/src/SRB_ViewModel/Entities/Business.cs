using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities;

public class Business
{
   [Key]
   public string BusinessID { get; set; } = null!;

   [Required]
   [StringLength(255)]
   public string BusinessName { get; set; } = string.Empty;

   [StringLength(100)]
   public string? TaxCode { get; set; }// Mã số thuế, có thể null nếu không có hoặc chưa cung cấp

   [Url]
   public string? Website { get; set; }

   [StringLength(500)]
   public string? LogoUrl { get; set; }

   [Required]
   [EmailAddress]
   public string ContactEmail { get; set; } = string.Empty;

   [Phone]
   [StringLength(20)]
   public string? PhoneNumber { get; set; }

   [StringLength(500)]
   public string? Address { get; set; }

   public string? Description { get; set; }

   public string? Industry { get; set; } // Ngành nghề

   public string? CompanySize { get; set; } // Quy mô (VD: 50-100 nhân viên)

   // Trạng thái xác thực
   public bool IsVerified { get; set; } = false;

   public string CreatedByID { get; set; } = null!;

   [ForeignKey(nameof(CreatedByID))]
   public virtual User CreatedBy { get; set; } = null!;

   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   public DateTime? UpdatedAt { get; set; }

   public virtual ICollection<User> Employees { get; set; } = [];
}
