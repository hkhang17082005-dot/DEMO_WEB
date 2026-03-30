using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel;
using SRB_WebPortal.Data;
using SRB_WebPortal.Models;

namespace SRB_WebPortal.Controllers;

public class HomeController(
   ILogger<HomeController> logger,
   DatabaseContext context
) : Controller
{
   private readonly ILogger<HomeController> _logger = logger;
   private readonly DatabaseContext _context = context;

   public IActionResult Index()
   {
      ViewBag.Locations = LocationMock.GetLocations();

      // Logic lưu trạng thái trái tim (Tạm thời để trống hoặc giữ nguyên nếu db SavedJobs đã có)
      ViewBag.SavedJobs = new List<string>();

      return View();
   }

   public IActionResult Privacy()
   {
      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

      return View(new ErrorViewModel(requestId, 500, "Đã có lỗi xảy ra"));
   }
}
