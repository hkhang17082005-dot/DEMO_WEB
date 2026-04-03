using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.user;

public interface IUserRepository
{
   Task<List<User>> GetAllAsync();
   Task<User?> GetByIdAsync(string userId);
   Task<User?> GetByUsernameAsync(string username);

   Task AddAsync(User user);
   Task UpdateAsync(User user);
   Task DeleteAsync(User user);

   Task<bool> ExistsAsync(string username);
}

