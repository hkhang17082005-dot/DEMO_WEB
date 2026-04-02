using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Data;
using SRB_WebPortal.Consts;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Services;

using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Controllers.apis.business;

namespace SRB_WebPortal.Shared;

public interface IShareRepository
{
   Task<bool> IsHasRole(string userID, string[] roleSlugs);
   Task<int?> GetRoleIDBySlug(string roleSlug);
   Task<IEnumerable<Location>> GetLocations();
   Task<JobPost?> GetJobPost(string jobPostID);
   Task<IEnumerable<JobPostApprovalDTO>> GetPendingJobPostsAsync(string jobPostID);
   Task<CVReviewDetailDTO?> GetApplicationDetailAsync(string applicationID);
   Task<List<MyApplicationDTO>> GetUserApplicationsAsync(string userID);
   Task<CandidateDashboardVM> GetDashboardStatsAsync(string userID);
   Task<(int ApprovedCount, int TotalApplications)> GetJobApplicationStats(string jobPostId);
   Task<(int ActivePosts, int NewCVs, int Interviews)> GetBusinessDashboardStats(string businessId);
   Task<List<ApprovedJobPostDTO>> GetApprovedJobPostsAsync(string? searchTerm = null, string? jobType = null, string? status = null);
}

public class ShareRepository(
   DatabaseContext context,
   IRedisService redisService
) : IShareRepository
{
   private readonly DatabaseContext _context = context;
   private readonly IRedisService _redisService = redisService;

   public async Task<List<ApprovedJobPostDTO>> GetApprovedJobPostsAsync(
      string? searchTerm = null,
      string? jobType = null,
      string? status = null
   )
   {
      try
      {
         var query = _context.JobPosts
            .Include(j => j.Business)
            .Include(j => j.Location)
            .Where(j => j.IsActive == true && j.ExpiryDate >= DateTime.UtcNow)
            .Select(j => new ApprovedJobPostDTO
            {
               JobPostID = j.JobPostID,
               Title = j.Title,
               Description = j.Description,
               Requirements = j.Requirements,
               Benefits = j.Benefits,
               SalaryRange = j.SalaryRange,
               JobType = j.JobType,
               JobTypeName = JobTypeHelper.GetName((int)j.JobType),
               IsActive = j.IsActive,
               ExpiryDate = j.ExpiryDate,
               CreatedAt = j.CreatedAt,
               Address = j.Address,
               BusinessName = j.Business != null ? j.Business.BusinessName : string.Empty,
               BusinessLogo = j.Business != null ? j.Business.LogoUrl : null,
               LocationName = j.Location != null ? j.Location.LocationName : string.Empty,
               TotalApplications = _context.JobApplications.Count(a => a.JobPostID == j.JobPostID)
            });

         // Apply search filter
         if (!string.IsNullOrWhiteSpace(searchTerm))
         {
            query = query.Where(j => j.Title.Contains(searchTerm) ||
                                     j.Description.Contains(searchTerm) ||
                                     j.BusinessName.Contains(searchTerm));
         }

         // Apply job type filter
         if (!string.IsNullOrWhiteSpace(jobType) && jobType != "All")
         {
            var jobTypeEnum = Enum.Parse<JobType>(jobType);
            query = query.Where(j => j.JobType == jobTypeEnum);
         }

         var jobPosts = await query
             .OrderByDescending(j => j.CreatedAt)
             .ToListAsync();

         // Lấy danh sách ứng viên cho từng JobPost
         foreach (var jobPost in jobPosts)
         {
            jobPost.Applicants = await GetApplicantsByJobPostAsync(jobPost.JobPostID);

            // Filter theo status nếu có
            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
               var statusEnum = Enum.Parse<ApplicationStatus>(status);
               jobPost.Applicants = jobPost.Applicants
                   .Where(a => a.Status == statusEnum)
                   .ToList();
            }
         }

         return jobPosts;
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetApprovedJobPostsAsync: {ex.Message}");
         return [];
      }
   }

   private async Task<List<ApplicationUserDTO>> GetApplicantsByJobPostAsync(string jobPostID)
   {
      try
      {
         var applicants = await _context.JobApplications
             .Include(a => a.User)
                 .ThenInclude(u => u.UserProfile)
             .Where(a => a.JobPostID == jobPostID)
             .Select(a => new ApplicationUserDTO
             {
                ApplicationID = a.ApplicationID,
                UserID = a.UserID,
                FullName = a.User != null && a.User.UserProfile != null
                     ? a.User.UserProfile.FullName
                     : "Không rõ",
                Email = a.User != null && a.User.UserProfile != null
                     ? a.User.UserProfile.Email
                     : "Không rõ",
                PhoneNumber = a.User != null && a.User.UserProfile != null
                     ? a.User.UserProfile.PhoneNumber
                     : null,
                AvatarURL = a.User != null && a.User.UserProfile != null
                     ? a.User.UserProfile.AvatarURL
                     : null,
                CVPath = a.CVPath,
                Summary = a.User != null && a.User.UserProfile != null
                     ? a.User.UserProfile.Summary
                     : null,
                Status = a.Status,
                StatusName = JobTypeHelper.GetApplicationStatusName((int)a.Status),
                AppliedAt = a.AppliedAt,
                Feedback = a.Feedback
             })
             .OrderByDescending(a => a.AppliedAt)
             .ToListAsync();

         return applicants;
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetApplicantsByJobPostAsync: {ex.Message}");

         return [];
      }
   }

   public async Task<(int ActivePosts, int NewCVs, int Interviews)> GetBusinessDashboardStats(string businessId)
   {
      var activePostsCount = await _context.JobPosts
          .CountAsync(p => p.BusinessID == businessId && p.IsActive);

      var newCVCount = await _context.JobApplications
          .CountAsync(a => a.JobPost.BusinessID == businessId && a.Status == ApplicationStatus.Submitted);

      var interviewCount = await _context.JobApplications
          .CountAsync(a => a.JobPost.BusinessID == businessId && a.Status == ApplicationStatus.Interviewing);

      return (activePostsCount, newCVCount, interviewCount);
   }

   public async Task<(int ApprovedCount, int TotalApplications)> GetJobApplicationStats(string jobPostId)
   {
      var stats = await _context.JobApplications
         .Where(a => a.JobPostID == jobPostId)
         .Select(a => a.Status)
         .ToListAsync();

      int total = stats.Count;

      int approved = stats.Count(s => s != ApplicationStatus.Submitted);

      return (approved, total);
   }

   public async Task<CandidateDashboardVM> GetDashboardStatsAsync(string userID)
   {
      var userProfile = await _context.UserProfiles
         .Where(u => u.UserID == userID)
         .FirstOrDefaultAsync();

      var stats = new CandidateDashboardVM
      {
         FullName = userProfile?.FullName ?? "Không rõ",
         ProfileCompletion = 75,
         TotalApplied = await _context.JobApplications.CountAsync(a => a.UserID == userID),
         TotalInterviews = await _context.JobApplications.CountAsync(a => a.UserID == userID && a.Status == ApplicationStatus.Interviewing),
         TotalSaved = await _context.SavedJobs.CountAsync(s => s.UserId == userID),
         TotalViews = 0,
      };

      return stats;
   }

   public async Task<List<MyApplicationDTO>> GetUserApplicationsAsync(string userID)
   {
      return await _context.JobApplications
         .Where(a => a.UserID == userID)
         .OrderByDescending(a => a.AppliedAt)
         .Select(a => new MyApplicationDTO
         {
            JobTitle = a.JobPost.Title,
            CompanyName = a.JobPost.Business.BusinessName,
            CompanyLogo = a.JobPost.Business.LogoUrl,
            AppliedAt = a.AppliedAt,
            CVFileName = a.CVPath,
            Status = a.Status,
            UpdatedAt = a.UpdatedAt
         })
         .ToListAsync();
   }

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
