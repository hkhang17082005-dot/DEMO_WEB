using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Consts;
using SRB_WebPortal.Services;

namespace SRB_WebPortal.Shared;

public interface IShareRepository
{
   Task<bool> IsHasRole(string userID, string[] roleSlugs);
   Task<int?> GetRoleIDBySlug(string roleSlug);
   Task<IEnumerable<Location>> GetLocations();
   Task<JobPost?> GetJobPost(string jobPostID);
}

public class ShareRepository(
   DatabaseContext context,
   IRedisService redisService
) : IShareRepository
{
   private readonly DatabaseContext _context = context;
   private readonly IRedisService _redisService = redisService;

   public async Task<JobPost?> GetJobPost(string jobPostID)
   {
      try
      {
         return await _context.JobPosts
            .Include(j => j.Business)
            .Include(j => j.Location)
            .FirstOrDefaultAsync(j => j.JobPostID == jobPostID);
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error: {ex.Message}");

         return null;
      }
   }

   public async Task<IEnumerable<Location>> GetLocations()
   {
      try
      {
         return await _context.Locations.AsNoTracking().ToListAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetLocations: {ex.Message}");

         return [];
      }
   }

   public async Task<bool> IsHasRole(string userID, string[] roleSlugs)
   {
      if (roleSlugs == null || roleSlugs.Length == 0) return false;

      return await _context.UserRoles
         .AsNoTracking()
         .AnyAsync(ur => ur.UserID == userID && roleSlugs.Contains(ur.Role.RoleSlug));
   }

   public async Task<int?> GetRoleIDBySlug(string roleSlug)
   {
      try
      {
         if (string.IsNullOrWhiteSpace(roleSlug)) return null;

         string roleKey = RedisCacheKeys.RoleIDBySlug(roleSlug);
         var cacheRoleID = await _redisService.GetAsync<int?>(roleKey);

         int finalRoleID;
         if (cacheRoleID == null)
         {
            finalRoleID = await _context.Roles
               .AsNoTracking()
               .Where(r => r.RoleSlug == roleSlug)
               .Select(r => r.RoleID)
               .FirstOrDefaultAsync();

            if (finalRoleID == -1)
            {
               return null;
            }

            await _redisService.SetAsync(roleKey, finalRoleID);
         }
         else
         {
            finalRoleID = cacheRoleID.Value;
         }

         return finalRoleID;
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetRoleIDBySlug: {ex.Message}");

         return null;
      }
   }
}
