using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Consts;

namespace SRB_WebPortal.Controllers.routes;

public class LoginController : Controller
{
   public IActionResult Index()
   {
      if (HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload)
      {
         return RedirectToAction("Index", "Home");
      }

      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

      return View(new ErrorViewModel(requestId, 500, "Đã có lỗi xảy ra"));
   }
}
