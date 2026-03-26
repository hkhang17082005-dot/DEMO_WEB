using System.ComponentModel.DataAnnotations;

namespace SRB_ViewModel.Entities;

public class Role
{
   [Key]
   public int RoleID { get; set; }

   public required string RoleName { get; set; }

   public required string RoleSlug { get; set; }

   public int TypeRoleID { get; set; }

   public string? RoleDesc { get; set; }

   public DateTime CreatedAt { get; set; }

   public DateTime UpdatedAt { get; set; }

   public virtual TypeRole TypeRole { get; set; } = null!;

   public virtual ICollection<UserRoles>? UserRoles { get; set; }

   public virtual ICollection<RolePermission>? RolePermissions { get; set; }
}
