using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.manager
{
   public class DashboardController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }
   }
}
