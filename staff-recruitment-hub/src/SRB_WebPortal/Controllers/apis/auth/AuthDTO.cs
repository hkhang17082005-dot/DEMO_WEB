using System.ComponentModel.DataAnnotations;

using SRB_ViewModel.Data;

namespace SRB_WebPortal.Controllers.apis.auth;

public class RegisterModelDTO
{
   [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
   public required string Username { get; set; }

   [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
   [DataType(DataType.Password)]
   [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
   public required string Password { get; set; }

   [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
   [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp")]
   [DataType(DataType.Password)]
   public required string ConfirmPassword { get; set; }
}

public class LoginModelDTO
{
   [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
   public required string Username { get; set; }

   [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
   [DataType(DataType.Password)]
   public required string Password { get; set; }

   public bool RememberMe { get; set; }
}

public class CreateProfileDTO
{
   public IFormFile? AvatarFile { get; set; }

   [Required(ErrorMessage = "Yêu cầu Họ và Tên")]
   [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
   public string FullName { get; set; } = string.Empty;

   [Required(ErrorMessage = "Email không được để trống")]
   [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
   public string Email { get; set; } = string.Empty;

   [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
   public string? PhoneNumber { get; set; }

   [Required(ErrorMessage = "Vui lòng chọn giới tính")]
   public string Gender { get; set; } = GenderTypes.Male;

   [DataType(DataType.Date)]
   public DateTime Birthday { get; set; }

   public string? Address { get; set; }

   public string? Summary { get; set; }

   public IFormFile? CVFile { get; set; }
}

public class UserMeResponse
{
   public required string UserID { get; set; }

   public required string Username { get; set; }

   public required string[] RoleSlugs { get; set; }

   public string? Email { get; set; }

   public string? FullName { get; set; }

   public string? PhoneNumber { get; set; }

   public string? BusinessID { get; set; }

   public string? AvatarURL { get; set; }

   public string? CVvURL { get; set; }
}
