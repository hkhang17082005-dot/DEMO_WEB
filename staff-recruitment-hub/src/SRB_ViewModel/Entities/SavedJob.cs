namespace SRB_ViewModel.Entities;

public class SavedJob
{
   public int Id { get; set; }

   public required string UserId { get; set; }

   public int JobID { get; set; }

   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   public virtual User? User { get; set; }

   public virtual JobPost? JobPost { get; set; }
}
