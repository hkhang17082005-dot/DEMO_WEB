using Microsoft.AspNetCore.Mvc;
using SRB_ViewModel;

namespace SRB_WebPortal.Controllers.routes;
public class Job_AppliedController : Controller
{
    private readonly DatabaseContext _context;
    public Job_AppliedController(DatabaseContext context)
    {
        _context = context;
    }   
   public IActionResult Index()
   {
      return View();
   }
}