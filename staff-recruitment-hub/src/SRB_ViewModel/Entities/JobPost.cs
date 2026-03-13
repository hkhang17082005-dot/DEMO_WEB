using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRB_ViewModel.Entities
{
    public class JobPost
    {
        [Key]
        public int JobPostID { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? SalaryRange { get; set; }

        public string? Location { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Khóa ngoại liên kết tới Công ty
        public int BusinessID { get; set; }

        [ForeignKey("BusinessID")]
        public virtual BusinessProfile Business { get; set; } = null!;
    }
}