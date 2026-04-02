using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.candidate
{
    // Class này đại diện cho từng object bên trong mảng "data" của JSON
    public class JobApplicationDTO
    {
        public string ApplicationID { get; set; } = null!;
        public string JobPostID { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public string? BusinessLogoUrl { get; set; }

        // Trạng thái ứng tuyển (số nguyên map với Enum ApplicationStatus)
        public int Status { get; set; }

        public DateTime AppliedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CVPath { get; set; }
    }
    public class CandidateProfileDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Summary { get; set; }
        // Tạm thời để trống JobTitle, sau này thêm cột vào DB thì hứng ở đây
        public string? JobTitle { get; set; } 
    }
}