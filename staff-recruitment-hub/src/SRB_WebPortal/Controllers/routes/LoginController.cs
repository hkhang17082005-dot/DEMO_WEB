using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.routes;

public class LoginController : Controller
{
   public IActionResult Index()
   {
      if (HttpContext.Items["SessionLogin"] is TokenPayload)
      {
         return RedirectToAction("Index", "Home");
      }

      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   }
}
