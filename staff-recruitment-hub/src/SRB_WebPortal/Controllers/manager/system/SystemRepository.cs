using Microsoft.EntityFrameworkCore;
using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.manager.system;

public interface ISystemRepository
{
   Task<Dictionary<string, int>> GetRoleIDsBySlugs(List<string> roleSlugs);
   Task UpdateUserRole(string userID, List<int> listRoleID);
}

public class SystemRepository(DatabaseContext context) : ISystemRepository
{
   private readonly DatabaseContext _context = context;

   public async Task<Dictionary<string, int>> GetRoleIDsBySlugs(List<string> roleSlugs)
   {
      try
      {
         if (roleSlugs == null || roleSlugs.Count == 0)
            return [];

         // Lấy tất cả Role có Slug nằm trong danh sách
         return await _context.Roles
            .AsNoTracking()
            .Where(r => roleSlugs.Contains(r.RoleSlug))
            .ToDictionaryAsync(r => r.RoleSlug, r => r.RoleID);
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetRoleIDsBySlugs: {ex.Message}");

         return [];
      }
   }

   public async Task UpdateUserRole(string userID, List<int> listRoleID)
   {
      using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
         // Tìm các Role hiện tại của User
         var existingUserRoles = await _context.UserRoles
            .Where(ur => ur.UserID == userID)
            .ToListAsync();

         // Xóa tất cả Role cũ
         if (existingUserRoles.Count != 0)
         {
            _context.UserRoles.RemoveRange(existingUserRoles);
         }

         // Thêm danh sách Role mới
         var newUserRoles = listRoleID.Select(roleId => new UserRoles
         {
            UserID = userID,
            RoleID = roleId,
            UpdatedAt = DateTime.UtcNow
         });

         await _context.UserRoles.AddRangeAsync(newUserRoles);

         await _context.SaveChangesAsync();

         await transaction.CommitAsync();
      }
      catch (Exception ex)
      {
         await transaction.RollbackAsync();

         Console.Error.WriteLine($"Error Update User Role for User {userID}: {ex.Message}");
         throw;
      }
   }
}
