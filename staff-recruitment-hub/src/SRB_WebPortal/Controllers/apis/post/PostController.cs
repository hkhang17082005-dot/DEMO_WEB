using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.apis.post;

[ApiController]
[Route("api/[controller]")]
public class PostController(IPostService postService) : BaseAPIController
{
   private readonly IPostService _postService = postService;

   [AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER })]
   [HttpGet("health")]
   public IActionResult Health()
   {
      return Ok("API Post Running");
   }

   [HttpPost("create-post")]
   [AuthGuard(Roles = new[] { Roles.BUSINESS_MANAGER, Roles.HIRING_MANAGER, Roles.HR_MANAGER })]
   public async Task<IActionResult> CreatePost([FromBody] CreateJobPostDTO formData)
   {
      if (IsManagerMissingBusiness(out var error)) return error;

      if (string.IsNullOrEmpty(CurrentUserID) || CurrentBusinessID != formData.BusinessID)
      {
         return Unauthorized(BaseResponse.Unauthorized("Không tìm thấy Thông tin cần thiết!"));
      }

      var result = await _postService.CreateNewPost(formData, CurrentUserID);

      if (!result.IsSuccess)
      {
         return BadRequest(result);
      }

      return Ok(result);
   }

   [HttpPost("upload-my-cv")]
   public async Task<IActionResult> UploadMyCV([FromForm] UploadCVModel model)
   {
      var result = await _postService.UploadCVAsync(model.FileCV);

      return Ok(result);
   }
}

