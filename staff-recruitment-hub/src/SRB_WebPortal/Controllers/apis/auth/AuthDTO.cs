using System.ComponentModel.DataAnnotations;

namespace SRB_WebPortal.Controllers.apis.auth
{
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
}
