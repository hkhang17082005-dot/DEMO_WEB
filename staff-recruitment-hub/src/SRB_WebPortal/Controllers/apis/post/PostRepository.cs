using Microsoft.EntityFrameworkCore;
using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.post;

public interface IPostRepository
{
   Task<string> CreateNewJobPost(JobPost jobPost);
   Task UpdateJobPost(JobPost jobPost);
   Task<List<JobPost>> GetPagedPosts(string? lastPostID, int pageSize = 10);
   Task<JobPost?> GetByID(string jobPostID);
}

public class PostRepository(DatabaseContext context) : IPostRepository
{
   private readonly DatabaseContext _context = context;

   public async Task<List<JobPost>> GetPagedPosts(string? lastPostID, int pageSize = 10)
   {
      var query = _context.JobPosts
         .AsNoTracking()
         .Where(p => p.IsActive);

      // Nếu có lastPostID, chỉ lấy những bài có ID lớn hơn (mới hơn)
      if (!string.IsNullOrEmpty(lastPostID))
      {
         query = query.Where(p => string.Compare(p.JobPostID, lastPostID) > 0);
      }

      return await query
         .OrderBy(p => p.JobPostID) // Sắp xếp tăng dần theo thời gian (UUIDv7)
         .Take(pageSize)
         .ToListAsync();
   }

   public async Task<string> CreateNewJobPost(JobPost jobPost)
   {
      try
      {
         jobPost.JobPostID = jobPost.JobPostID;

         jobPost.IsActive = true;
         jobPost.CreatedAt = DateTime.UtcNow;

         await _context.JobPosts.AddAsync(jobPost);

         await _context.SaveChangesAsync();

         return jobPost.JobPostID;
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in Create New JobPost: {ex.Message}");

         return "";
      }
   }

   public async Task UpdateJobPost(JobPost jobPost)
   {
      try
      {
         _context.Entry(jobPost).State = EntityState.Modified;

         await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error updating JobPost: {ex.Message}");

         throw;
      }
   }

   public async Task<JobPost?> GetByID(string jobPostID)
   {
      try
      {
         return await _context.JobPosts.FirstOrDefaultAsync(x => x.JobPostID == jobPostID);
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in Get JobPost By ID: {ex.Message}");

         return null;
      }
   }

}
