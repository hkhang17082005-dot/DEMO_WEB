using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.manager.system;

[Route("manager/system/ManagerSystem")]
[AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER, Roles.SUPPORT })]
public class ManagerSystemController : Controller
{
   public IActionResult Index()
   {
      return View();
   }
}
