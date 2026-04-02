using Microsoft.AspNetCore.Mvc;
using SRB_ViewModel;

namespace SRB_WebPortal.Controllers.routes;

public class Job_AppliedController(DatabaseContext context) : Controller
{
   private readonly DatabaseContext _context = context;

   public IActionResult Index()
   {
      return View();
   }
}
