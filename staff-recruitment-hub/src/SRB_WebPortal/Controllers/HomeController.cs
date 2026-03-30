using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel;
using SRB_WebPortal.Data;
using SRB_WebPortal.Models;

using SRB_WebPortal.Controllers.apis.post;

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

   public IActionResult GetJobs(string keyword, int? locationId, int page = 1)
   {
      int pageSize = 6;

      // Lấy dữ liệu từ Mock
      var allJobs = JobMock.GetJobs();
      var query = allJobs.AsQueryable();

      // --- LOGIC TÌM KIẾM (Search In-Memory) ---
      if (!string.IsNullOrEmpty(keyword))
      {
         // Sử dụng OrdinalIgnoreCase để tìm kiếm không phân biệt hoa thường khi chạy trên List
         query = query.Where(j =>
             j.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
             (j.Description != null && j.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
         );
      }

      if (locationId.HasValue)
      {
         query = query.Where(j => j.LocationID == locationId);
      }

      // --- LOGIC PHÂN TRANG ---
      int totalItems = query.Count();
      int totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize);
      if (totalPages == 0) totalPages = 1;

      // Đảm bảo page nằm trong khoảng hợp lệ
      page = page < 1 ? 1 : (page > totalPages ? totalPages : page);

      var jobs = query
          .OrderByDescending(j => j.JobPostID) // Sắp xếp theo ID (Guid string)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      // --- TRUYỀN DỮ LIỆU RA PARTIAL VIEW ---
      ViewBag.CurrentPage = page;
      ViewBag.TotalPages = totalPages;
      ViewBag.Keyword = keyword;
      ViewBag.CurrentLocation = locationId;

      // Tạm thời để trống SavedJobs nếu bạn chưa làm phần lưu vào Database cho Mock
      ViewBag.SavedJobs = new List<string>();

      var jobDtos = jobs.Select(p => new JobPostDTO
      {
         JobPostID = p.JobPostID,
         Title = p.Title,
         BusinessID = p.BusinessID,
         BusinessName = "Business VNG",
         BusinessLogoURL = "/images/business-logo.png",
         JobType = p.JobType,
         SalaryRange = p.SalaryRange ?? "Thỏa thuận",
         LocationID = p.LocationID,
         Address = p.Address ?? "Toàn quốc",
         CreatedAt = p.CreatedAt
      }).ToList();

      return PartialView("_JobListPartial", jobDtos);
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

      return View(new ErrorViewModel(requestId, 500, "Đã có lỗi xảy ra"));
   }
}
