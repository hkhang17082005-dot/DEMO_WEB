using SRB_WebPortal.Shared;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.Apis.Business
{
   public class BusinessService(BusinessRepository repo)
   {
        public async Task<BaseResponse<BusinessProfile>> GetProfile(string userId)
        {
            var data = await repo.GetByUserId(userId);
            return BaseResponse<BusinessProfile>.Success(data!, "Lấy thông tin thành công");
        }
        public async Task<BaseResponse<List<JobPost>>> GetMyJobs(string userId)
        {
            var profile = await repo.GetByUserId(userId);
            if (profile == null) return BaseResponse<List<JobPost>>.Failure("Không tìm thấy doanh nghiệp");

            var jobs = await repo.GetJobsByBusinessId(profile.Id);
            return BaseResponse<List<JobPost>>.Success(jobs, "Lấy danh sách tin tuyển dụng thành công");
        }
   }
}