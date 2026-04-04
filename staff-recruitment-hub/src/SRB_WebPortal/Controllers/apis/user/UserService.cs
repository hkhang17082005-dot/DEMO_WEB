using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.user;

public class UserService(IUserRepository repo) : IUserService
{
   private readonly IUserRepository _repo = repo;

   public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
   {
      var user = new User
      {
         UserID = Guid.NewGuid().ToString(),
         Username = dto.Username,
         HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
         Status = UserStatus.active,
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow
      };
      await _repo.AddAsync(user);

      return new UserResponseDto
      {
         UserID = user.UserID,
         Username = user.Username,
         Status = user.Status.ToString(),
         Email = dto.Email,
         RoleName = "User"
      };
   }

   public Task<bool> DeleteUserAsync(string id)
   {
      throw new NotImplementedException();
   }

   public async Task<List<UserResponseDto>> GetAllUsersAsync()
   {
      var users = await _repo.GetAllAsync();
      return [.. users.Select(u => new UserResponseDto
      {
         UserID = u.UserID,
         Username = u.Username,
         Status = u.Status.ToString(),
         Email = u.UserProfile?.Email,
         RoleName = u.UserRoles.FirstOrDefault()?.Role.RoleName ?? "Unknown"
      })];
   }

   public Task<UserResponseDto?> GetUserByIdAsync(string id)
   {
      throw new NotImplementedException();
   }

   public Task<UserResponseDto?> GetUserByUsernameAsync(string username)
   {
      throw new NotImplementedException();
   }

   public Task<UserResponseDto?> LockUserAsync(string id)
   {
      throw new NotImplementedException();
   }

   public Task<UserResponseDto?> UpdateUserAsync(string id, UserUpdateDto dto)
   {
      throw new NotImplementedException();
   }
   // Lấy tất cả candidates (roleID 50 là Candidate)
   public async Task<List<UserResponseDto>> GetCandidatesAsync()
   {
      var users = await _repo.GetAllAsync();

      var candidates = users.Where(u => u.UserRoles.Any(ur => ur.RoleID == 50));
      
      return candidates.Select(u => new UserResponseDto
      {
         UserID = u.UserID,
         Username = u.Username,
         Status = u.Status.ToString(),
         Email = u.UserProfile?.Email,
         RoleName = "Candidate"
      }).ToList();
   }
}
