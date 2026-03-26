namespace SRB_ViewModel.Entities;

public class RolePermission
{
   public int RoleID { get; set; }

   public int PerID { get; set; }

   public DateTime CreatedAt { get; set; }

   public DateTime UpdatedAt { get; set; }

   public virtual Role Role { get; set; } = null!;

   public virtual Permission Permission { get; set; } = null!;

}
