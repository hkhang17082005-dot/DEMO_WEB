using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_WebPortal.Consts;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Services;

using SRB_WebPortal.Controllers.apis.business;
using SRB_ViewModel.Data;

namespace SRB_WebPortal.Shared;

public interface IShareRepository
{
   Task<bool> IsHasRole(string userID, string[] roleSlugs);
   Task<int?> GetRoleIDBySlug(string roleSlug);
   Task<IEnumerable<Location>> GetLocations();
   Task<JobPost?> GetJobPost(string jobPostID);
   Task<IEnumerable<JobPostApprovalDTO>> GetPendingJobPostsAsync(string jobPostID);
   Task<CVReviewDetailDTO?> GetApplicationDetailAsync(string applicationID);
}

public class ShareRepository(
   DatabaseContext context,
   IRedisService redisService
) : IShareRepository
{
   private readonly DatabaseContext _context = context;
   private readonly IRedisService _redisService = redisService;

   public async Task<CVReviewDetailDTO?> GetApplicationDetailAsync(string applicationID)
   {
      try
      {
         return await _context.JobApplications
            .Include(a => a.JobPost)
            .Include(a => a.User)
               .ThenInclude(u => u.UserProfile)
            .Where(a => a.ApplicationID == applicationID)
            .Select(a => new CVReviewDetailDTO
            {
               ApplicationID = a.ApplicationID,
               UserID = a.UserID,
               FullName = (a.User != null && a.User.UserProfile != null) ? a.User.UserProfile.FullName : "Không rõ",
               Email = (a.User != null && a.User.UserProfile != null) ? a.User.UserProfile.Email : "Không rõ",
               Phone = (a.User != null && a.User.UserProfile != null) ? a.User.UserProfile.PhoneNumber ?? "Không rõ" : "Không rõ",
               JobTitle = a.JobPost.Title,
               CVPath = a.CVPath,
               CoverLetter = a.CoverLetter,
               AppliedAt = a.AppliedAt,
               Status = a.Status,
               StatusName = JobTypeHelper.GetApplicationStatusName((int)a.Status)
            })
            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetApplicationDetailAsync: {ex.Message}");

         return null;
      }
   }

   public async Task<IEnumerable<JobPostApprovalDTO>> GetPendingJobPostsAsync(string jobPostID)
   {
      var pendingApplications = await _context.JobApplications
         .Include(a => a.User)
         .ThenInclude(u => u.UserProfile)
         .Where(a => a.JobPostID == jobPostID && a.Status == ApplicationStatus.Submitted)
         .OrderByDescending(a => a.AppliedAt)
         .Select(a => new JobPostApprovalDTO
         {
            ApplicationID = a.ApplicationID,
            JobPostID = a.JobPostID,
            Title = a.JobPost.Title,
            UserID = a.UserID,
            CandidateName = (a.User != null && a.User.UserProfile != null) ? a.User.UserProfile.FullName : "Không rõ",
            CandidateEmail = (a.User != null && a.User.UserProfile != null) ? a.User.UserProfile.Email : "Không rõ",
            CVPath = a.CVPath,
            AppliedAt = a.AppliedAt,
            Status = a.Status
         })
         .ToListAsync();

      return pendingApplications;
   }

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
