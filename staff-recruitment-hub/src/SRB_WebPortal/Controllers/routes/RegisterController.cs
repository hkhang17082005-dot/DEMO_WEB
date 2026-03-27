using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Consts;

namespace SRB_WebPortal.Controllers.routes;

public class RegisterController : Controller
{
   public IActionResult Index()
   {
      if (HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload)
      {
         return RedirectToAction("Index", "Home");
      }

      return View();
   }

   public IActionResult CreateProfile()
   {
      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   }
}
