using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Utils;

public static class MailTemplateHelper
{
   public static (string Subject, string Body) GetTemplate(ApplicationStatus status, string? FullName, string jobTitle)
   {
      string name = FullName ?? "Bạn";
      string job = jobTitle ?? "vị trí đã ứng tuyển";

      return status switch
      {
         ApplicationStatus.Interviewing => (
            "Thư mời phỏng vấn - [SRB Platform]",
            $"Hi {name}, chúc mừng bạn! Hồ sơ ứng tuyển vị trí <b>{job}</b> đã được thông qua. Chúng tôi muốn mời bạn một buổi phỏng vấn..."
         ),
         ApplicationStatus.Rejected => (
            "Thông báo kết quả ứng tuyển - [SRB Platform]",
            $"Chào {name}, cảm ơn bạn đã quan tâm vị trí <b>{job}</b>. Rất tiếc hiện tại chúng tôi chưa thể đồng hành cùng bạn..."
         ),
         ApplicationStatus.Offered => (
            "Thư mời nhận việc (Offer Letter) - [SRB Platform]",
            $"Chúc mừng {name}! Bạn đã xuất sắc vượt qua các vòng tuyển dụng cho vị trí <b>{job}</b>..."
         ),
         _ => ("Cập nhật trạng thái hồ sơ", $"Chào {name}, hồ sơ của bạn cho vị trí {job} đã được cập nhật trạng thái mới.")
      };
   }
}
