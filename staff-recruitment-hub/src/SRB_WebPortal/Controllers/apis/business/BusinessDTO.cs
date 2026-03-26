using System.ComponentModel.DataAnnotations;

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
