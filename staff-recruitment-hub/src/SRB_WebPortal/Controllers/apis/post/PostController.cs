using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.apis.post
{
   [Route("api/[controller]")]
   [ApiController]
   public class PostController(PostService postService) : Controller
   {
      private readonly PostService _postService = postService;

      [HttpGet("health")]
      public IActionResult Health()
      {
         return Ok("API Post Running");
      }

      [HttpPost("crete-post")]
      public async Task<IActionResult> CreatePost()
      {
         return Ok("Create New Post Successful");
      }

      [HttpPost("upload-my-cv")]
      public async Task<IActionResult> UploadMyCV([FromForm] UploadCVModel model)
      {
         var result = await _postService.UploadCVAsync(model.FileCV);

         return Ok(result);
      }

      [HttpPut("update-post")]
      [AuthGuard(Roles = new[] { "admin", "system_manager" })]
      public async Task<IActionResult> UpdatePost()
      {
         var result = _postService.UpdatePost();

         return Ok(result);
      }
   }
}
