using System.ComponentModel.DataAnnotations;

namespace SRB_WebPortal.Controllers.apis.post
{
   public class CreatePost
   {
      [Required(ErrorMessage = "Tiêu đề bài đăng không được để trống")]
      [Length(10, 50, ErrorMessage = "Tiêu đề phải trong khoảng 10 đến 50 kí tự")]
      public required string PostTile { get; set; }
   }

   public class UploadCVModel
   {
      [Required(ErrorMessage = "File CV không phù hợp hoặc không tồn tại")]
      public required IFormFile FileCV { get; set; }
   }
}
