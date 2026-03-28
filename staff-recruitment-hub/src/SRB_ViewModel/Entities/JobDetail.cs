using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities
{
    public class JobDetail
    {
        [Key]
        [ForeignKey("Job")]
        public int JobID { get; set; } // Vừa là khóa chính, vừa là khóa ngoại

        public string? Description { get; set; } // Mô tả công việc (Rich Text/HTML)
        
        public string? Requirements { get; set; } // Yêu cầu ứng viên
        
        public string? Benefits { get; set; } // Quyền lợi
        
        public string? WorkAddress { get; set; } // Địa chỉ cụ thể (số nhà, đường...)
        
        public string? WorkTime { get; set; } // Thời gian làm việc (Ví dụ: Thứ 2 - Thứ 6)
        
        public int Quantity { get; set; } // Số lượng tuyển dụng
        
        public string? Experience { get; set; } // Yêu cầu kinh nghiệm (Ví dụ: 1-2 năm)
        
        public string? Gender { get; set; } // Yêu cầu giới tính
        
        public DateTime? Deadline { get; set; } // Hạn chót nộp hồ sơ

        // Navigation property
        public virtual Job Job { get; set; } = null!;
    }
}