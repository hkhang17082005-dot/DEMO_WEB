using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.Job;

public class JobPostRepository : IJobPostRepository
{
   private readonly DatabaseContext _context;

   public JobPostRepository(DatabaseContext context)
   {
      _context = context;
   }

   public async Task<List<JobPost>> GetAllAsync() =>
await _context.JobPosts
             .Include(j => j.Business)
             .Include(j => j.Location)
             .ToListAsync();


   public async Task<JobPost?> GetByIdAsync(string id) =>
       await _context.JobPosts
                     .Include(j => j.Business)
                     .Include(j => j.Location)
                     .FirstOrDefaultAsync(j => j.JobPostID == id);

   public async Task AddAsync(JobPost post)
   {
      _context.JobPosts.Add(post);
      await _context.SaveChangesAsync();
   }

   public async Task UpdateAsync(JobPost post)
   {
      _context.JobPosts.Update(post);
      await _context.SaveChangesAsync();
   }

   public async Task DeleteAsync(JobPost post)
   {
      _context.JobPosts.Remove(post);
      await _context.SaveChangesAsync();
   }
}
