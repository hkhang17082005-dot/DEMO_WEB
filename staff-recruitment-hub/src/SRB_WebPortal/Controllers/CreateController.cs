using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers;

public class CreateController : Controller
{
   // GET: /create
   public IActionResult Index()
   {
      // uses Views/Create/Index.cshtml if present
      return View();
   }
}
