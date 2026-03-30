using System.Net;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.post;

public interface IPostService
{
   Task<BaseResponse<SaveJobPostRes>> SavePost(SaveJobPostDTO formData, string ownerID, string? businessID);
   Task<BaseResponse<List<JobPostDTO>>> LoadListPost(string? lastPostID, int pageSize);
   Task<BaseResponse> ApplyJobPost(string JobPostID);
}

public class PostService(IPostRepository postRepository) : IPostService
{
   private readonly IPostRepository _postRepository = postRepository;

   public async Task<BaseResponse<List<JobPostDTO>>> LoadListPost(string? lastPostID, int pageSize)
   {
      if (!string.IsNullOrEmpty(lastPostID) && lastPostID.Length != 36)
      {
         return BaseResponse<List<JobPostDTO>>.Failure("Mã định danh không phù hợp", HttpStatusCode.BadRequest);
      }

      var posts = await _postRepository.GetPagedPosts(lastPostID, pageSize);

      var data = posts.Select(p => new JobPostDTO
      {
         JobPostID = p.JobPostID,
         Title = p.Title,
         BusinessID = p.BusinessID,
         BusinessName = p.Business?.BusinessName,
         BusinessLogoURL = p.Business?.LogoUrl ?? "default-logo.png",
         JobType = p.JobType,
         SalaryRange = p.SalaryRange ?? "Không rõ",
         LocationID = p.LocationID,
         Address = p.Address,
         CreatedAt = p.CreatedAt,
      }).ToList();

      return BaseResponse<List<JobPostDTO>>.Success(data, "Tải danh sách thành công");
   }

   public async Task<BaseResponse<SaveJobPostRes>> SavePost(SaveJobPostDTO formData, string ownerID, string? businessID)
   {
      string jobPostRes;
      if (!string.IsNullOrEmpty(formData.JobPostID))
      {
         // Update Post
         var existingPost = await _postRepository.GetByID(formData.JobPostID);
         if (existingPost == null)
         {
            return BaseResponse<SaveJobPostRes>.Failure("Không tìm thấy bài đăng để cập nhật", HttpStatusCode.NotFound);
         }

         // Cập nhật các Field
         existingPost.UpdatedByID = ownerID;
         existingPost.Title = formData.Title;
         existingPost.LocationID = formData.LocationID;
         existingPost.Description = formData.Description;
         existingPost.Requirements = formData.Requirements;
         existingPost.Benefits = formData.Benefits;
         existingPost.SalaryRange = formData.SalaryRange;
         existingPost.Address = formData.Address;
         existingPost.UpdatedAt = DateTime.Now;

         await _postRepository.UpdateJobPost(existingPost);

         jobPostRes = existingPost.JobPostID;
      }
      else
      {
         // Create Post
         if (string.IsNullOrEmpty(businessID))
         {
            return BaseResponse<SaveJobPostRes>.Failure("Không tìm thấy thông tin doanh nghiệp!", HttpStatusCode.NotFound);
         }

         var newPost = new JobPost
         {
            JobPostID = Guid.NewGuid().ToString(),
            BusinessID = businessID,
            Title = formData.Title,
            Description = formData.Description,
            Requirements = formData.Requirements ?? "Yêu cầu mặc định",
            Benefits = formData.Benefits,
            SalaryRange = formData.SalaryRange,
            LocationID = formData.LocationID,
            Address = formData.Address,
            CreatedAt = DateTime.Now,
            CreatedByID = ownerID,
            IsActive = true
         };

         jobPostRes = await _postRepository.CreateNewJobPost(newPost);
      }

      var savePostRes = new SaveJobPostRes(jobPostRes);

      return BaseResponse<SaveJobPostRes>.Success(savePostRes, "Lưu bài đăng tuyển thành công");
   }

   public async Task<BaseResponse> ApplyJobPost(string JobPostID)
   {
      return BaseResponse.Success("Update Post Successful..");
   }
}
