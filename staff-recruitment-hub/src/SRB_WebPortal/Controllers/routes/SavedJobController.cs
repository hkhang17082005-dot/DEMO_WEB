using SRB_ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SRB_WebPortal.Controllers.routes;

public class SavedJobController(DatabaseContext context) : Controller
{
   private readonly DatabaseContext _context = context;

   public IActionResult Index()
   {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if (userId == null)
         return Redirect("/Login");

      var savedJobs = _context.SavedJobs
         .Where(x => x.UserId == userId)
         .Include(x => x.Job)
         .ThenInclude(j => j!.Location)
         .Select(x => x.Job)
         .ToList();

      return View(savedJobs);
   }
}
