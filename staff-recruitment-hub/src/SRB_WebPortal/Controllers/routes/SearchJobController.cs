using SRB_ViewModel;
using SRB_ViewModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.routes;

public class FindJobByIDController : Controller
{
   private readonly DatabaseContext _context;

   public FindJobByIDController(DatabaseContext context)
   {
      _context = context;
   }

   // SEARCH JOB
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

      ViewBag.Keyword = keyword;
      ViewBag.CurrentLocation = locationId;
      ViewBag.Locations = _context.Locations.ToList();

      return View(jobs.ToList()); // trả về Search.cshtml
   }

   // JOB BY LOCATION
   /*   public IActionResult GetJobById(int id)
      {
          var jobs = _context.Jobs
              .Include(j => j.Location)
              .Where(j => j.LocationID == id)
              .ToList();

          ViewBag.Location = _context.Locations
              .Where(l => l.LocationID == id)
              .Select(l => l.LocationName)
              .FirstOrDefault();

          ViewBag.CurrentLocation = id;

          ViewBag.Locations = _context.Locations.ToList();

          return View(jobs);
      }  */
}
