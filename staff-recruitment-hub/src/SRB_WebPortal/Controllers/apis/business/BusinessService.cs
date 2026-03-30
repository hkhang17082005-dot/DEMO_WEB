using SRB_WebPortal.Shared;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Controllers.apis.post;

namespace SRB_WebPortal.Controllers.apis.business;

public class BusinessService(IBusinessRepository businessRepository)
{
   private readonly IBusinessRepository _businessRepo = businessRepository;

   public async Task<BaseResponse> RegisterBusiness(RegisterBusinessDTO formData, string userID)
   {
      bool userHasBusiness = await _businessRepo.ExistingUserBusiness(userID);
      if (userHasBusiness)
      {
         return BaseResponse.Conflict("Bạn đã có Công ty rồi không thể đăng ký");
      }

      await _businessRepo.RegisterBusiness(formData, userID);

      return BaseResponse.Success("Register Business Successful!");
   }

   public async Task<BaseResponse<Business>> GetProfile(string userId)
   {
      var data = await _businessRepo.GetByUserId(userId);
      if (data == null)
         return BaseResponse<Business>.Failure("Không tìm thấy thông tin doanh nghiệp");

      return BaseResponse<Business>.Success(data, "Lấy thông tin thành công");
   }

   public async Task<BaseResponse<List<JobPost>>> GetMyJobs(string userId)
   {
      var profile = await _businessRepo.GetByUserId(userId);

      if (profile == null) return BaseResponse<List<JobPost>>.Failure("Không tìm thấy doanh nghiệp");

      var jobs = await _businessRepo.GetJobsByBusinessId(profile.BusinessID);

      return BaseResponse<List<JobPost>>.Success(jobs, "Lấy danh sách tin tuyển dụng thành công");
   }

   public async Task<BaseResponse<IEnumerable<JobPost>>> GetBusinessJobPost(GetJobPostDTO requestData, string businessID)
   {
      if (string.IsNullOrEmpty(requestData.LastPostID))
      {
         requestData.LastPostID = null;
      }

      var jobs = await _businessRepo.GetBusinessJobPosts(requestData.LastPostID, requestData.GetSize, businessID);

      return BaseResponse<IEnumerable<JobPost>>.Success(jobs, "Lấy danh sách tin tuyển dụng thành công");
   }
}
