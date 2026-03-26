using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.manager;

[AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER, Roles.SUPPORT })]
[Route("manager/[controller]/[action]")]
public class ManageBusinessController : Controller
{
   public IActionResult Index()
   {
      return View();
   }
}

