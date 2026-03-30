using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Consts;
using SRB_WebPortal.Extensions;

using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;

namespace SRB_WebPortal.Controllers.apis.business;

[ApiController]
[Route("api/[controller]")]
public class BusinessController(BusinessService businessService) : BaseAPIController
{
   private readonly BusinessService _businessService = businessService;

   [HttpGet("health")]
   [AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER })]
   public async Task<IActionResult> Health()
   {
      return Ok("Controller API is Running!");
   }

   [HttpGet("profile")]
   public async Task<IActionResult> GetProfile()
   {
      var tokenPayload = HttpContext.GetItem<TokenPayload>(ServerKey.CONTEXT_ITEM_TOKEN_INFO);

      if (tokenPayload is null)
      {
         return RedirectToAction("Index");
      }

      var result = await _businessService.GetProfile(tokenPayload.User.UserID);

      return Ok(result);
   }

   [HttpGet("my-jobs")]
   public async Task<IActionResult> GetMyJobs()
   {
      var session = HttpContext.GetItem<SessionInfo>(ServerKey.CONTEXT_ITEM_TOKEN_INFO);

      if (session is null)
      {
         return RedirectToAction("Index");
      }

      var result = await _businessService.GetMyJobs(session.Payload.User.UserID);

      return Ok(result);
   }

   [HttpPost("register")]
   public async Task<IActionResult> RegisterBusiness([FromBody] RegisterBusinessDTO fromData)
   {
      if (string.IsNullOrEmpty(CurrentUserID)) return Unauthorized("Không tìm thấy Thông tin cần thiết!");

      var result = await _businessService.RegisterBusiness(fromData, CurrentUserID);

      return HandleResult(result);
   }

   [HttpGet("get-business-jp")]
   public async Task<IActionResult> GetBusinessJobPosts([FromQuery] GetJobPostDTO requestData)
   {
      if (string.IsNullOrEmpty(CurrentUserID) || string.IsNullOrEmpty(CurrentBusinessID))
      {
         return Unauthorized("Không tìm thấy Thông tin cần thiết!");
      }

      var result = await _businessService.GetBusinessJobPost(requestData, CurrentBusinessID);

      return HandleResult(result);
   }
}
