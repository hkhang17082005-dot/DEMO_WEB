// <summary>
//     ViewModel for creating a new job posting.    
// </summary>
using System.ComponentModel.DataAnnotations;

namespace SRB_WebPortal.Models;

public class CreateJobViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập chức danh")]
    public string Title { get; set; } = null!;

    public string? Industry { get; set; }
    public string? Location { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }

    [Required]
    public DateTime Deadline { get; set; }

    public string? Description { get; set; }
}