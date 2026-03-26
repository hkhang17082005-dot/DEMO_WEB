using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Shared;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers;

public abstract class BaseAPIController : ControllerBase
{
   protected string CurrentUserId =>
      HttpContext.Items["SessionInfo"] is SessionInfo session
      ? session.Payload.User.UserID
      : string.Empty;

   protected SessionInfo? CurrentSession => HttpContext.Items["SessionInfo"] as SessionInfo;

   protected IActionResult HandleResult(BaseResponse result)
   {
      if (result == null) return NotFound();

      if (result.IsSuccess) return Ok(result);

      return result.StatusCode switch
      {
         400 => BadRequest(result),
         401 => Unauthorized(result),
         403 => Forbid(),
         404 => NotFound(result),
         409 => Conflict(result),
         _ => StatusCode(500, result)
      };
   }
}
