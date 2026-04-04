namespace SRB_WebPortal.Controllers.apis.user;

public interface IUserService
{
   Task<List<UserResponseDto>> GetAllUsersAsync();
   Task<UserResponseDto?> GetUserByIdAsync(string id);
   Task<UserResponseDto?> GetUserByUsernameAsync(string username);

   Task<UserResponseDto> CreateUserAsync(UserCreateDto dto);
   Task<UserResponseDto?> UpdateUserAsync(string id, UserUpdateDto dto);
   Task<bool> DeleteUserAsync(string id);

   Task<UserResponseDto?> LockUserAsync(string id);
   Task <List<UserResponseDto>> GetCandidatesAsync();
}

