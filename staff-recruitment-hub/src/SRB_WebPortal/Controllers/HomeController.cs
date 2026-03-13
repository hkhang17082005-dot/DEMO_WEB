using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SRB_WebPortal.Models;
using SRB_ViewModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace SRB_WebPortal.Controllers
{
   public class HomeController(ILogger<HomeController> logger, DatabaseContext context) : Controller
   {
      private readonly ILogger<HomeController> _logger = logger;
      private readonly DatabaseContext _context = context;
      public IActionResult Index()
      {
         var jobs = _context.Jobs
                .Include(j => j.Location)
                .ToList();
         ViewBag.Locations = _context.Locations.ToList();
         return View(jobs);
      }

      public IActionResult Privacy()
      {
         return View();
      }

      [HttpGet]
      public IActionResult Search(string keyword, int? locationId)
      {
         var jobs = _context.Jobs
             .Include(j => j.Location)
             .AsQueryable();

         if (!string.IsNullOrEmpty(keyword))
         {
            jobs = jobs.Where(j =>
                j.Title.Contains(keyword) ||
                j.CompanyName.Contains(keyword));
         }

         if (locationId.HasValue)
         {
            jobs = jobs.Where(j => j.LocationID == locationId);
         }

         var result = jobs.Select(j => new
         {
            title = j.Title,
            companyName = j.CompanyName,
            salary = j.Salary,
            jobType = j.JobType,
            location = new
            {
               locationName = j.Location.LocationName
            }
         }).ToList();

         return Json(result);
      }



      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
