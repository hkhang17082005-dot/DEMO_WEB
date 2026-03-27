using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Shared;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Consts;

namespace SRB_WebPortal.Controllers;

public abstract class BaseAPIController : ControllerBase
{
   protected string CurrentUserID =>
      HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload tokenPayload
      ? tokenPayload.User.UserID
      : string.Empty;

   protected string? CurrentBusinessID =>
      HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload tokenPayload
      ? tokenPayload.User.BusinessID
      : string.Empty;

   protected bool IsManagerMissingBusiness(out IActionResult error)
   {
      if (string.IsNullOrEmpty(CurrentBusinessID))
      {
         error = Unauthorized(BaseResponse.Unauthorized("Tài khoản chưa liên kết với doanh nghiệp!"));
         return true;
      }

      error = null!;
      return false;
   }

   protected TokenPayload? CurrentPayload => HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] as TokenPayload;

   protected IActionResult HandleResult<T>(BaseResponse<T> result)
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
         _ => StatusCode(result.StatusCode != 0 ? result.StatusCode : 500, result)
      };
   }

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
