using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.routes;

public class BlogController : Controller
{
   public IActionResult Index() => View();
}
