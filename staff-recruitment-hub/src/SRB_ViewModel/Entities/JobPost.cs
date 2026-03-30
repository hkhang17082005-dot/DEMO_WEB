using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities;

public class JobPost
{
   [Key]
   public string JobPostID { get; set; } = null!;

   [Required]
   public string Title { get; set; } = null!;

   [Required]
   public string Description { get; set; } = null!;

   [Required]
   public string Requirements { get; set; } = null!;

   public string? Benefits { get; set; }

   public string? SalaryRange { get; set; }

   public JobType JobType { get; set; } = JobType.FullTime;

   public bool IsActive { get; set; } = true;

   public string CreatedByID { get; set; } = null!;

   public string? UpdatedByID { get; set; }

   public DateTime ExpiryDate { get; set; }

   public DateTime CreatedAt { get; set; }

   public DateTime UpdatedAt { get; set; }

   public string? Address { get; set; }

   public int LocationID { get; set; }

   [ForeignKey("LocationID")]
   public virtual Location Location { get; set; } = null!;

   public string BusinessID { get; set; } = null!;

   [ForeignKey("BusinessID")]
   public virtual Business Business { get; set; } = null!;
}

public enum JobType
{
   FullTime = 1,      // Toàn thời gian
   PartTime = 2,      // Bán thời gian
   Contract = 3,      // Hợp đồng (Freelance)
   Internship = 4,    // Thực tập
   Remote = 5,        // Làm việc từ xa
   Hybrid = 6,        // Kết hợp (Lên văn phòng + Tại nhà)
   Freelance = 7,     // Tự do
   Temporary = 8      // Thời vụ/Tạm thời
}
