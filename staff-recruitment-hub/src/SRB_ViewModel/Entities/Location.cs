using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities;

public class Location
{
   [Key]
   public int LocationID { get; set; }

   public required string LocationName { get; set; }

   public ICollection<Job>? Jobs { get; set; }
}
