using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities;

public class JobApplication
{
   [Key]
   public string ApplicationID { get; set; } = null!;

   public string UserID { get; set; } = null!; // Người ứng tuyển

   public string JobPostID { get; set; } = null!; // Bài đăng được ứng tuyển

   [Required]
   public string CVPath { get; set; } = null!;

   public string? CoverLetter { get; set; }

   public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

   public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

   // Ghi chú của HR/Hiring Manager sau khi xem CV
   public string? Feedback { get; set; }

   [ForeignKey("UserID")]
   public virtual User User { get; set; } = null!;

   [ForeignKey("JobPostID")]
   public virtual JobPost JobPost { get; set; } = null!;
}

public enum ApplicationStatus
{
   Submitted = 0,    // Đã nộp
   UnderReview = 1,  // Đang xem xét
   Interviewing = 2, // Đang phỏng vấn
   Offered = 3,      // Đã mời làm việc
   Rejected = 4,     // Bị từ chối
   Withdrawn = 5     // Ứng viên tự rút đơn
}
