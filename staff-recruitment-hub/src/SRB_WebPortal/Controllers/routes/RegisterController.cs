using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SRB_WebPortal.Models;

namespace SRB_WebPortal.Controllers.routes
{
   public class RegisterController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
