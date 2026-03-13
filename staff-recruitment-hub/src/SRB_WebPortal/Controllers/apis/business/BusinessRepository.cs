using Microsoft.EntityFrameworkCore;
using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.Apis.Business
{
   public class BusinessRepository(DatabaseContext context)
   {
        public async Task<BusinessProfile?> GetByUserId(string userId)
        {
            return await context.Users
                .Where(u => u.UserID == userId)
                .Select(u => u.Business)
                .FirstOrDefaultAsync();
        }

        public async Task<List<JobPost>> GetJobsByBusinessId(int businessId)
        {
            // Giả sử bạn có bảng JobPosts liên kết với BusinessProfile qua BusinessID
            return await context.Set<JobPost>()
                .Where(j => j.BusinessID == businessId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }
   }
}