using SRB_WebPortal.Shared;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Services;
using SRB_WebPortal.Utils;

namespace SRB_WebPortal.Controllers.apis.business;

public class BusinessService(
   IBusinessRepository businessRepository,
   IResendService resendService)
{
   private readonly IBusinessRepository _businessRepo = businessRepository;
   private readonly IResendService _mailService = resendService;

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

   public async Task<BaseResponse<IEnumerable<JobPostDTO>>> GetBusinessJobPost(GetJobPostDTO requestData, string businessID)
   {
      if (string.IsNullOrEmpty(requestData.LastPostID))
      {
         requestData.LastPostID = null;
      }

      var jobs = await _businessRepo.GetBusinessJobPosts(requestData.LastPostID, requestData.GetSize, businessID);

      return BaseResponse<IEnumerable<JobPostDTO>>.Success(jobs, "Lấy danh sách tin tuyển dụng thành công");
   }

   public async Task<BaseResponse> UpdateStatusApplyJob(UpdateStatusApplyJobDTO formData)
   {
      try
      {
         var application = await _businessRepo.UpdateApplicationStatusAsync(formData.ApplicationID, formData.Status);

         if (application == null)
         {
            return BaseResponse.BadRequest("Không tìm thấy hồ sơ ứng tuyển hoặc cập nhật thất bại!");
         }

         string fullname = application.User?.UserProfile?.FullName ?? "Bạn";
         string jobTitle = application.JobPost.Title;
         string? userEmail = application.User?.UserProfile?.Email;

         var (subject, body) = MailTemplateHelper.GetTemplate(application.Status, fullname, jobTitle);

         if (userEmail is not null && !string.IsNullOrEmpty(userEmail))
         {
            await _mailService.SendMailAsync(userEmail, subject, body);
         }

         return BaseResponse.Success("Cập nhật trạng thái ứng viên thành công");
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in UpdateStatusApplyJob: {ex.Message}");

         return BaseResponse.BadRequest("Đã xảy ra lỗi hệ thống khi cập nhật!");
      }
   }

}
