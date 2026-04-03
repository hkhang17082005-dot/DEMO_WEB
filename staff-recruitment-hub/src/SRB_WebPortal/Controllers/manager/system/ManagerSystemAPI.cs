using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.manager.system;

[ApiController]
[Route("api/[controller]")]
[AuthGuard(Roles = new[] { Roles.ADMIN })]
public class ManagerSystemAPI(ISystemService systemService) : BaseAPIController
{
   private readonly ISystemService _systemService = systemService;

   [HttpGet("test")]
   [AuthGuard(Roles = new[] { Roles.SUPPORT })]
   public IActionResult TestAPI()
   {
      return Ok(BaseResponse.Success("Test API successful"));
   }

   [HttpPost("update-role")]
   public async Task<IActionResult> UpdateUserRole([FromBody] UpdateRoleRequest requestData)
   {
      if (requestData.RoleSlugs.Length == 0)
      {
         BadRequest("Dữ liệu không hợp lệ!");
      }

      if (string.IsNullOrEmpty(CurrentUserID))
      {
         return Unauthorized(BaseResponse.Unauthorized("Không tìm thấy Thông tin cần thiết!"));
      }

      var result = await _systemService.UpdateUserRole(CurrentUserID, requestData);

      return HandleResult(result);
   }
}
