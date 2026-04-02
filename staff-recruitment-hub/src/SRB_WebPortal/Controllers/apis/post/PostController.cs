using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Services;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Consts;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.post;

[ApiController]
[Route("api/[controller]")]
public class PostController(
   IPostService postService,
   IBunnyCNDService bunnyCNDService,
   IServiceScopeFactory serviceScopeFactory
) : BaseAPIController
{
   private readonly IPostService _postService = postService;

   private readonly IBunnyCNDService _bunnyCNDService = bunnyCNDService;

   private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

   [AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER })]
   [HttpGet("health")]
   public IActionResult Health()
   {
      return Ok("API Post Running");
   }

   [IsPublic]
   [HttpGet("load-list")]
   public async Task<IActionResult> LoadListPost([FromQuery] LoadJobPostsRequest requestData)
   {
      var result = await _postService.LoadListPost(requestData.LastPostID, requestData.PageSize);

      return HandleResult(result);
   }

   [HttpGet("test-api")]
   public void TestAPI()
   {
      Console.WriteLine("NQHxLog: Controller Test API is Running!");

      return;
   }

   [HttpPost("apply-job-post")]
   public async Task<BaseResponse> ApplyJobPost([FromForm] ApplyJobPostReq requestData)
   {
      var userID = CurrentUserID;
      if (string.IsNullOrEmpty(userID) || requestData.CVFile == null || requestData.CVFile.Length == 0)
      {
         return BaseResponse.BadRequest("Dữ liệu không hợp lệ");
      }

      var ms = new MemoryStream();
      await requestData.CVFile.CopyToAsync(ms);
      ms.Position = 0; // Reset con trỏ về đầu stream

      _ = Task.Run(async () =>
      {
         using var scope = _scopeFactory.CreateScope();
         var postServiceInsideTask = scope.ServiceProvider.GetRequiredService<IPostService>();
         var bunnyServiceInsideTask = scope.ServiceProvider.GetRequiredService<IBunnyCNDService>();

         using (ms)
         {
            try
            {
               var uploadResult = await bunnyServiceInsideTask.UploadToBunnyRunBackground(ms, requestData.CVFile.FileName, CloudCNDKey.FOLDER_APPLY_JOB_CV);

               if (uploadResult.IsSuccess)
               {
                  var applyJobPost = new JobApplication
                  {
                     ApplicationID = Guid.CreateVersion7().ToString(),
                     JobPostID = requestData.JobPostID,
                     UserID = userID,
                     CVPath = uploadResult?.Data?.ToString() ?? string.Empty,
                     Status = ApplicationStatus.Submitted,
                     AppliedAt = DateTime.Now
                  };

                  await postServiceInsideTask.ApplyJobPost(applyJobPost);
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Background Upload Failed for JobID: {requestData.JobPostID} - Error: {ex}");
            }
         }
      });

      return BaseResponse.Success("Bài đăng đang được xử lý!");
   }

   [HttpPost("save-post")]
   [AuthGuard(Roles = new[] { Roles.BUSINESS_MANAGER, Roles.HIRING_MANAGER, Roles.HR_MANAGER })]
   public async Task<IActionResult> SavePost([FromBody] SaveJobPostDTO formData)
   {
      if (IsManagerMissingBusiness(out var error)) return error;

      if (string.IsNullOrEmpty(CurrentUserID))
      {
         return Unauthorized(BaseResponse.Unauthorized("Không tìm thấy thông tin cần thiết!"));
      }

      var result = await _postService.SavePost(formData, CurrentUserID, CurrentBusinessID);

      if (!result.IsSuccess)
      {
         return BadRequest(result);
      }

      return Ok(result);
   }

   [HttpPost("upload-cv")]
   public async Task<IActionResult> UploadCVGetURL([FromForm] UploadCVModel model)
   {
      var result = await _bunnyCNDService.UploadToBunny(model.FileCV, CloudCNDKey.FOLDER_APPLY_JOB_CV);

      return HandleResult(result);
   }

   [AuthGuard(Roles = new[] { Roles.ADMIN })]
   [HttpDelete("delete-file-cv")]
   public async Task<IActionResult> DeleteFileCV([FromBody] DeleteFileRequest requestData)
   {
      var result = await _bunnyCNDService.DeleteFileAsync(requestData.FileName);

      return HandleResult(result);
   }

}

