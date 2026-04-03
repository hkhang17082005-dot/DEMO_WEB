using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.Job;

public interface IJobPostService
{
   Task<List<JobPost>> GetAllAsync();
   Task<JobPost?> GetByIdAsync(string id);
   Task<JobPost> CreateAsync(JobPost post);
   Task<JobPost?> UpdateAsync(JobPost post);
   Task<bool> DeleteAsync(string id);

   Task<JobPost?> ToggleApprovalAsync(string id); // Admin duyệt / từ chối
}
