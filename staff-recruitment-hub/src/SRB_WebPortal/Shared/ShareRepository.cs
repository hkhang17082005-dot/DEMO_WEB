using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;

namespace SRB_WebPortal.Shared;

public interface IShareRepository
{
   Task<bool> IsHasRole(string userID, string[] roleSlugs);
}

public class ShareRepository(DatabaseContext context) : IShareRepository
{
   private readonly DatabaseContext _context = context;

   public async Task<bool> IsHasRole(string userID, string[] roleSlugs)
   {
      if (roleSlugs == null || roleSlugs.Length == 0) return false;

      return await _context.UserRoles
         .AsNoTracking()
         .AnyAsync(ur => ur.UserID == userID && roleSlugs.Contains(ur.Role.RoleSlug));
   }
}
