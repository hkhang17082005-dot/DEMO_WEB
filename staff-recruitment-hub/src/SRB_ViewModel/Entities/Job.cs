namespace SRB_ViewModel.Entities
{
   public class Job
   {
      public int JobID { get; set; }

      public required string Title { get; set; }

      // Todo: Tách Company Ra thành 1 bảng riêng
      public required string CompanyName { get; set; }

      public required string Salary { get; set; }

      public required string JobType { get; set; }

      public int LocationID { get; set; }

      public Location Location { get; set; } = null!;

      public virtual ICollection<SavedJob>? SavedJobs { get; set; }
   }
}
