using System.ComponentModel.DataAnnotations;

namespace STAFF_RECRUITMENT_HUB.src.SRB_WebPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }       // Vị trí tuyển dụng

        public string CompanyName { get; set; } // Tên công ty

        public string Salary { get; set; }      // Lương

        public string Location { get; set; }    // Địa điểm

        public string JobType { get; set; }     // Full-time / Part-time
    }
}