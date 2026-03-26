using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.routes;

public class ServicesController : Controller
{
   public IActionResult Index()
   {
      return View();
   }
}

