using System.ComponentModel.DataAnnotations;

using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.business;

public class RegisterBusinessDTO
{
   [Required(ErrorMessage = "Tên doanh nghiệp không được để trống")]
   [StringLength(255)]
   public string BusinessName { get; set; } = string.Empty;

   [StringLength(100)]
   public string? TaxCode { get; set; }

   [Url(ErrorMessage = "Định dạng Website không hợp lệ")]
   public string? Website { get; set; }

   [Required(ErrorMessage = "Email liên hệ là bắt buộc")]
   [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
   public string ContactEmail { get; set; } = string.Empty;

   [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
   [StringLength(20)]
   public string? PhoneNumber { get; set; }

   [StringLength(500)]
   public string? Address { get; set; }

   public string? Description { get; set; }

   public string? Industry { get; set; }

   public string? CompanySize { get; set; }

}

public class UpdateBusinessDTO
{
   public string CompanyName { get; set; } = null!;
   public string? Website { get; set; }
}

public class ApprovedJobPostDTO
{
   public string JobPostID { get; set; } = string.Empty;

   public string Title { get; set; } = string.Empty;

   public string Description { get; set; } = string.Empty;

   public string Requirements { get; set; } = string.Empty;

   public string? Benefits { get; set; }

   public string? SalaryRange { get; set; }

   public JobType JobType { get; set; }

   public string JobTypeName { get; set; } = string.Empty;

   public bool IsActive { get; set; }

   public DateTime ExpiryDate { get; set; }

   public DateTime CreatedAt { get; set; }

   public string? Address { get; set; }

   public string BusinessName { get; set; } = string.Empty;

   public string? BusinessLogo { get; set; }

   public string LocationName { get; set; } = string.Empty;

   public int TotalApplications { get; set; }

   public List<ApplicationUserDTO> Applicants { get; set; } = [];
}

public class ApplicationUserDTO
{
   public string ApplicationID { get; set; } = string.Empty;

   public string UserID { get; set; } = string.Empty;

   public string FullName { get; set; } = string.Empty;

   public string Email { get; set; } = string.Empty;

   public string? PhoneNumber { get; set; }

   public string? AvatarURL { get; set; }

   public string? CVPath { get; set; }

   public string? Summary { get; set; }

   public ApplicationStatus Status { get; set; }

   public string StatusName { get; set; } = string.Empty;

   public DateTime AppliedAt { get; set; }

   public string? Feedback { get; set; }
}

public class JobPostApprovalDTO
{
   public required string ApplicationID { get; set; }
   public required string JobPostID { get; set; }
   public required string Title { get; set; }
   public required string UserID { get; set; }
   public string CandidateName { get; set; } = null!;
   public string CandidateEmail { get; set; } = null!;
   public required string CVPath { get; set; }
   public DateTime AppliedAt { get; set; }
   public ApplicationStatus Status { get; set; }
}

public class CVReviewDetailDTO
{
   public required string ApplicationID { get; set; }
   public required string UserID { get; set; }
   public string FullName { get; set; } = null!;
   public string Email { get; set; } = null!;
   public string Phone { get; set; } = null!;
   public required string JobTitle { get; set; }
   public required string CVPath { get; set; }
   public string? CoverLetter { get; set; }
   public DateTime AppliedAt { get; set; }
   public ApplicationStatus Status { get; set; }
   public required string StatusName { get; set; }
}

public record UpdateStatusApplyJobDTO(
   string ApplicationID,
   ApplicationStatus Status
);
