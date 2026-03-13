using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities
{
   public class Permission
   {
      [Key]
      public int PerID { get; set; }

      public string PerName { get; set; } = null!;

      public string PerSlug { get; set; } = null!;

      public string? PerDesc { get; set; }

      public DateTime CreatedAt { get; set; }

      public DateTime UpdatedAt { get; set; }

      public virtual ICollection<RolePermission>? RolePermissions { get; set; }
   }
}
