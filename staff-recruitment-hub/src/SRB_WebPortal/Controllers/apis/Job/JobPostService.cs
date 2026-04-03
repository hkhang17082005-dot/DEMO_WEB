using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.Job;

public class JobPostService : IJobPostService
{
   private readonly IJobPostRepository _repo;

   public JobPostService(IJobPostRepository repo)
   {
      _repo = repo;
   }

   public async Task<List<JobPost>> GetAllAsync() => await _repo.GetAllAsync();

   public async Task<JobPost?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

   public async Task<JobPost> CreateAsync(JobPost post)
   {
      post.JobPostID = Guid.NewGuid().ToString();
      post.CreatedAt = DateTime.UtcNow;
      post.UpdatedAt = DateTime.UtcNow;
      post.IsActive = false; // mặc định chờ duyệt
      await _repo.AddAsync(post);
      return post;
   }

   public async Task<JobPost?> UpdateAsync(JobPost post)
   {
      var existing = await _repo.GetByIdAsync(post.JobPostID);
      if (existing == null) return null;

      existing.Title = post.Title;
      existing.Description = post.Description;
      existing.Requirements = post.Requirements;
      existing.UpdatedAt = DateTime.UtcNow;

      await _repo.UpdateAsync(existing);
      return existing;
   }


   public async Task<bool> DeleteAsync(string id)
   {
      var post = await _repo.GetByIdAsync(id);
      if (post == null) return false;
      await _repo.DeleteAsync(post);
      return true;
   }

   public async Task<JobPost?> ToggleApprovalAsync(string id)
   {
      var post = await _repo.GetByIdAsync(id);
      if (post == null) return null;

      post.IsActive = !post.IsActive;
      post.UpdatedAt = DateTime.UtcNow;
      await _repo.UpdateAsync(post);
      return post;
   }
}
