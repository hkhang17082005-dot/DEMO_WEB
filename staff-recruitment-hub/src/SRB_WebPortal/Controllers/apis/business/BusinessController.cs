using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Extensions;
using SRB_WebPortal.Controllers.apis.auth;

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
      var session = HttpContext.GetItem<SessionInfo>("SessionInfo"); // Lấy session từ Redis

      if (session is null)
      {
         return RedirectToAction("Index");
      }

      var result = await _businessService.GetProfile(session.Payload.User.UserID);

      return Ok(result);
   }

   [HttpGet("my-jobs")]
   public async Task<IActionResult> GetMyJobs()
   {
      var session = HttpContext.GetItem<SessionInfo>("SessionInfo");

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
      if (string.IsNullOrEmpty(CurrentUserId)) return Unauthorized("Không tìm thấy Thông tin cần thiết!");

      var result = await _businessService.RegisterBusiness(fromData, CurrentUserId);

      return HandleResult(result);
   }
}
