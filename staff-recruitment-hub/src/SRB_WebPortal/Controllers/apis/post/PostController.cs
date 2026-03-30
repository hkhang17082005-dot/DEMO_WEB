using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Services;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.apis.post;

[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService postService, IBunnyCNDService bunnyCNDService) : BaseAPIController
{
   private readonly IPostService _postService = postService;

   private readonly IBunnyCNDService _bunnyCNDService = bunnyCNDService;

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

   [HttpPost("apply-job-post")]
   public async Task<IActionResult> ApplyJobPost([FromQuery] ApplyJobPostReq requestData)
   {
      var result = await _postService.ApplyJobPost(requestData.JobPostID);

      return HandleResult(result);
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
      var result = await _bunnyCNDService.UploadToBunny(model.FileCV);

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

