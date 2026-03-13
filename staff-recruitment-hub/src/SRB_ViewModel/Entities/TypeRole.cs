using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities
{
   public class TypeRole
   {
      [Key]
      public int TypeRoleID { get; set; }

      public required string TypeName { get; set; }

      public required string TypeSlug { get; set; }

      public string? TypeDesc { get; set; }

      public DateTime CreatedAt { get; set; }

      public DateTime UpdatedAt { get; set; }

      public virtual ICollection<Role>? Roles { get; set; }
   }
}
