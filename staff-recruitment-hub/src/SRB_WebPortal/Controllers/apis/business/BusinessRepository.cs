using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Data;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.business;

public interface IBusinessRepository
{
   Task<Business?> GetByUserId(string userID);

   Task<List<JobPost>> GetJobsByBusinessId(string businessID);

   Task RegisterBusiness(RegisterBusinessDTO formData, string userID);

   Task<bool> ExistingUserBusiness(string userID);

   Task<IEnumerable<JobPost>> GetBusinessJobPosts(string? lastPostID, int postSize, string businessID);

   Task<bool> UpdateApplicationStatusAsync(string applicationId, ApplicationStatus status);
}

public class BusinessRepository(
   DatabaseContext context,
   IShareRepository shareRepository
) : IBusinessRepository
{
   private readonly DatabaseContext _context = context;
   private readonly IShareRepository _shareRepository = shareRepository;

   public async Task<bool> UpdateApplicationStatusAsync(string applicationId, ApplicationStatus status)
   {
      var application = await _context.JobApplications
         .FirstOrDefaultAsync(x => x.ApplicationID == applicationId);

      if (application == null) return false;

      application.Status = status;
      application.UpdatedAt = DateTime.Now;

      return await _context.SaveChangesAsync() > 0;
   }

   public async Task<bool> ExistingUserBusiness(string userID)
   {
      return await _context.Users
        .AnyAsync(u => u.UserID == userID && u.BusinessID != null);
   }

   public async Task RegisterBusiness(RegisterBusinessDTO formData, string userID)
   {
      using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
         string newBusinessID = Guid.CreateVersion7().ToString();
         var newBusiness = new Business
         {
            BusinessID = newBusinessID,
            BusinessName = formData.BusinessName,
            ContactEmail = formData.ContactEmail,
            PhoneNumber = formData.PhoneNumber,
            CreatedByID = userID,
            Address = formData.Address,
            CompanySize = formData.CompanySize,
            Description = formData.Description,
            Industry = formData.Industry,
            IsVerified = false,
            TaxCode = formData.TaxCode,
            Website = formData.Website
         };
         _context.Businesses.Add(newBusiness);

         var user = await _context.Users.FindAsync(userID);
         user?.BusinessID = newBusinessID;

         var roleBusinessManagerID = await _shareRepository.GetRoleIDBySlug(Roles.BUSINESS_MANAGER);
         if (roleBusinessManagerID is null)
         {
            await transaction.RollbackAsync();
            return;
         }

         var newUserRole = new UserRoles
         {
            UserID = userID,
            RoleID = roleBusinessManagerID.Value
         };

         await _context.UserRoles.AddAsync(newUserRole);

         await _context.SaveChangesAsync();
         await transaction.CommitAsync();
      }
      catch
      {
         await transaction.RollbackAsync();
         throw;
      }
   }

   public async Task<Business?> GetByUserId(string userID)
   {
      return await _context.Users
         .Where(u => u.UserID == userID)
         .Select(u => u.Business)
         .FirstOrDefaultAsync();
   }

   public async Task<List<JobPost>> GetJobsByBusinessId(string businessID)
   {
      // Giả sử bạn có bảng JobPosts liên kết với Business qua BusinessID
      return await _context.Set<JobPost>()
         .Where(j => j.BusinessID == businessID)
         .OrderByDescending(j => j.CreatedAt)
         .ToListAsync();
   }

   public async Task<IEnumerable<JobPost>> GetBusinessJobPosts(string? lastPostID, int postSize, string businessID)
   {
      try
      {
         var query = _context.JobPosts
            .AsNoTracking()
            .Where(j => j.BusinessID == businessID)
            .OrderByDescending(j => j.CreatedAt);

         if (!string.IsNullOrEmpty(lastPostID))
         {
            var lastPost = await _context.JobPosts.FindAsync(lastPostID);
            if (lastPost != null)
            {
               query = (IOrderedQueryable<JobPost>)query.Where(j => j.CreatedAt < lastPost.CreatedAt);
            }
         }

         return await query.Take(postSize).ToListAsync();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetBusinessJobPosts: {ex.Message}");
         throw;
      }
   }

}
