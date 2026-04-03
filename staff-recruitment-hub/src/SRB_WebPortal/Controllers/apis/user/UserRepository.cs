using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.user;

public class UserRepository(DatabaseContext context) : IUserRepository
{
   private readonly DatabaseContext _context = context;

   public async Task<List<User>> GetAllAsync() =>
        await _context.Users.Include(u => u.UserProfile)
                            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                            .ToListAsync();

   public async Task<User?> GetByIdAsync(string id) =>
       await _context.Users.Include(u => u.UserProfile)
                           .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                           .FirstOrDefaultAsync(u => u.UserID == id);

   public async Task<User?> GetByUsernameAsync(string username) =>
       await _context.Users.Include(u => u.UserProfile)
                           .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                           .FirstOrDefaultAsync(u => u.Username == username);

   public async Task AddAsync(User user)
   {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
   }

   public async Task UpdateAsync(User user)
   {
      _context.Users.Update(user);
      await _context.SaveChangesAsync();
   }

   public async Task DeleteAsync(User user)
   {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
   }

   public async Task<bool> ExistsAsync(string username)
   {
      return await _context.Users.AnyAsync(u => u.Username == username);
   }
}
