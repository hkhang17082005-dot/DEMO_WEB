using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.Job;

public interface IJobPostRepository
{
   Task<List<JobPost>> GetAllAsync();
   Task<JobPost?> GetByIdAsync(string id);
   Task AddAsync(JobPost post);
   Task UpdateAsync(JobPost post);
   Task DeleteAsync(JobPost post);
}
