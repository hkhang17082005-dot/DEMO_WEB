using System.ComponentModel.DataAnnotations;

namespace SRB_WebPortal.Controllers.apis.post;

public class CreateJobPostDTO
{
   [Required(ErrorMessage = "Mã định danh Doanh nghiệp không được để trống")]
   public string BusinessID { get; set; } = string.Empty;

   [Required(ErrorMessage = "Tiêu đề bài đăng không được để trống")]
   [StringLength(100, MinimumLength = 10, ErrorMessage = "Tiêu đề phải từ 10 đến 100 ký tự")]
   public string Title { get; set; } = string.Empty;

   [Range(1, 1000, ErrorMessage = "Số lượng tuyển dụng phải từ 1 đến 1000")]
   public int Quantity { get; set; }

   [Required(ErrorMessage = "Mức lương không được để trống")]
   [StringLength(100, MinimumLength = 10, ErrorMessage = "Mức lương từ 10 đến 100 ký tự")]
   public string SalaryRange { get; set; } = string.Empty;

   [Required(ErrorMessage = "Địa điểm không được để trống")]
   [StringLength(255, ErrorMessage = "Địa điểm quá dài")]
   public string Location { get; set; } = string.Empty;

   [Required(ErrorMessage = "Mô tả công việc không được để trống")]
   [MinLength(20, ErrorMessage = "Mô tả công việc quá ngắn (tối thiểu 20 ký tự)")]
   public string Description { get; set; } = string.Empty;

   public string Requirements { get; set; } = string.Empty;

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
