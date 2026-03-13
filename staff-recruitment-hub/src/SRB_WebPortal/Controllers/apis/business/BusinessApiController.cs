using Microsoft.AspNetCore.Mvc; // Xóa using Microsoft.AspNetCore.Components
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Extensions;

namespace SRB_WebPortal.Controllers.Apis.Business;

[Route("api/[controller]")]
[ApiController]
[AuthGuard(Roles = new[] { "BUSINESS_MANAGER", "HIRING_MANAGER" })] // Chỉ cho phép Business
public class BusinessApiController(BusinessService service) : ControllerBase
{
   [HttpGet("profile")]
   public async Task<IActionResult> GetProfile()
   {
      var session = HttpContext.GetItem<SessionInfo>("SessionInfo"); // Lấy session từ Redis
      var result = await service.GetProfile(session.Payload.User.UserID);
      return Ok(result);
   }

   [HttpGet("my-jobs")]
   public async Task<IActionResult> GetMyJobs()
   {
      var session = HttpContext.GetItem<SessionInfo>("SessionInfo"); //
      var result = await service.GetMyJobs(session.Payload.User.UserID);
      return Ok(result);
   }
}