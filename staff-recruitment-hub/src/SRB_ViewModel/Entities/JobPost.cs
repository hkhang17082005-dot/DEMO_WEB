using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities;

public class JobPost
{
   [Key]
   public string JobPostID { get; set; } = null!;

   [Required]
   public string Title { get; set; } = null!;

   public string? Description { get; set; }

   public string? SalaryRange { get; set; }

   public string? Location { get; set; }

   public bool IsActive { get; set; } = true;

   public string CreatedByID { get; set; } = null!;

   public string? UpdatedByID { get; set; }

   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

   public string BusinessID { get; set; } = null!;

   [ForeignKey("BusinessID")]
   public virtual Business Business { get; set; } = null!;
}

