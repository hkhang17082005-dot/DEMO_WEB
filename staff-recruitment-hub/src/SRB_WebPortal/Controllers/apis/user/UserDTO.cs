using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.user;

public class UserResponseDto
{
   public required string UserID { get; set; }
   public required string Username { get; set; }
   public required string Status { get; set; }
   public string? Email { get; set; }
   public required string RoleName { get; set; }
}

public class UserCreateDto
{
   public required string Username { get; set; }
   public required string Password { get; set; }
   public int RoleID { get; set; }
   public string? Email { get; set; }
}

public class UserUpdateDto
{
   public required string Username { get; set; }
   public UserStatus Status { get; set; }
   public int RoleID { get; set; }
}
