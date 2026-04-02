using System.ComponentModel.DataAnnotations;

using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.post;

public record ApplyJobPostReq(
   [Required(ErrorMessage = "Job ID không được để trống")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Job ID phải đúng định dạng")]
   string JobPostID,

   IFormFile CVFile
);

public class GetJobPostDTO
{
   public string? LastPostID { get; set; }
   public int GetSize { get; set; } = 5;
}

public class LoadJobPostsRequest
{
   public string? LastPostID { get; set; } = null;
   public int PageSize { get; set; } = 10;
}

public record JobPostDTO
{
   public string JobPostID { get; init; } = null!;

   public string Title { get; init; } = null!;

   public string? BusinessID { get; init; }

   public string? BusinessName { get; init; }

   public string? BusinessLogoURL { get; init; }

   public string? Description { get; init; }

   public string? Requirements { get; init; }

   public string? Benefits { get; init; }

   public JobType JobType { get; init; }

   public string? SalaryRange { get; init; }

   public int? LocationID { get; init; }

   public string? Address { get; init; }

   public DateTime ExpiryDate { get; init; }

   public DateTime? CreatedAt { get; init; }

   public int ApprovedCount { get; set; } = 0;

   public int TotalApplications { get; set; } = 0;
}

public record SaveJobPostRes(
   string JobPostID
);

public class SaveJobPostDTO
{
   public string? JobPostID { get; set; }

   [Required(ErrorMessage = "Tiêu đề bài đăng không được để trống")]
   [StringLength(100, MinimumLength = 10, ErrorMessage = "Tiêu đề phải từ 10 đến 100 ký tự")]
   public string Title { get; set; } = string.Empty;

   [Required(ErrorMessage = "Mức lương không được để trống")]
   [StringLength(100, MinimumLength = 8, ErrorMessage = "Mức lương từ 8 đến 100 ký tự")]
   public string SalaryRange { get; set; } = string.Empty;

   [Required(ErrorMessage = "Khu vực tuyển dụng không được trống")]
   public int LocationID { get; init; }

   [Required(ErrorMessage = "Địa điểm không được để trống")]
   [StringLength(255, ErrorMessage = "Địa điểm quá dài")]
   public string Address { get; set; } = string.Empty;

   [Required(ErrorMessage = "Mô tả công việc không được để trống")]
   [MinLength(20, ErrorMessage = "Mô tả công việc quá ngắn (tối thiểu 20 ký tự)")]
   public string Description { get; set; } = string.Empty;

   [Required(ErrorMessage = "Yêu cầu công việc không được để trống")]
   [MinLength(20, ErrorMessage = "Yêu cầu công việc quá ngắn (tối thiểu 20 ký tự)")]
   public string Requirements { get; set; } = string.Empty;

   [Required(ErrorMessage = "Đãi ngộ công việc không được để trống")]
   [MinLength(20, ErrorMessage = "Đãi ngộ công việc quá ngắn (tối thiểu 20 ký tự)")]
   public string Benefits { get; set; } = string.Empty;

   [EmailAddress(ErrorMessage = "Định dạng email liên hệ không hợp lệ")]
   public string? ContactEmail { get; set; }

   [DataType(DataType.Date)]
   public DateTime? ExpiryDate { get; set; }
}

public class UploadCVModel
{
   [Required(ErrorMessage = "File CV không phù hợp hoặc không tồn tại")]
   public required IFormFile FileCV { get; set; }
}

public record DeleteFileRequest(
   [Required(ErrorMessage = "FileName không được để trống")] string FileName
);

public class MyApplicationDTO
{
   public required string JobTitle { get; set; }
   public required string CompanyName { get; set; }
   public string? CompanyLogo { get; set; }
   public DateTime AppliedAt { get; set; }
   public string? CVFileName { get; set; }
   public ApplicationStatus Status { get; set; }
   public DateTime? UpdatedAt { get; set; }
}

public class CandidateDashboardVM
{
   public required string FullName { get; set; }
   public int ProfileCompletion { get; set; }
   public int TotalApplied { get; set; }
   public int TotalSaved { get; set; }
   public int TotalViews { get; set; }
   public int TotalInterviews { get; set; }
   public int SuitableJobsCount { get; set; }
}
