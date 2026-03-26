using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.routes;

public class PortfolioController : Controller
{
   public IActionResult Index() => View();
}
