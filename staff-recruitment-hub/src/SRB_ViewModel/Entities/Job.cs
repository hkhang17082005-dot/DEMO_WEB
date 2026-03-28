using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities
{
   public class Job
   {
      public string? LogoUrl { get; set; }   
      [Key]
      public int JobID { get; set; }

      public required string Title { get; set; }

      public required string CompanyName { get; set; }

      public required string Salary { get; set; }

      public required string JobType { get; set; }

      public int LocationID { get; set; }

      public Location Location { get; set; } = null!;

      // Navigation Property cho quan hệ 1-1
      public virtual JobDetail? JobDetail { get; set; }

      public virtual ICollection<SavedJob>? SavedJobs { get; set; }
   }
}