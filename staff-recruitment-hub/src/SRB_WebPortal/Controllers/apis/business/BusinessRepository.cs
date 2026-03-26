using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.business;

public interface IBusinessRepository
{
   Task<Business?> GetByUserId(string userID);

   Task<List<JobPost>> GetJobsByBusinessId(string businessID);

   Task RegisterBusiness(RegisterBusinessDTO formData, string userID);

   Task<bool> ExistingUserBusiness(string userID);
}

public class BusinessRepository(DatabaseContext context) : IBusinessRepository
{
   private readonly DatabaseContext _context = context;

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
}
