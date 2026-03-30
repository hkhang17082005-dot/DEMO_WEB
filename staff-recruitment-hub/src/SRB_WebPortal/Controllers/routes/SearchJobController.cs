using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Data;

namespace SRB_WebPortal.Controllers.routes;

public class FindJobByIDController : Controller
{
   // SEARCH JOB
   public IActionResult Search(string keyword, int? locationId)
   {
      // Lấy toàn bộ dữ liệu từ Mock
      var allJobs = JobMock.GetJobs();
      var query = allJobs.AsQueryable();

      // --- LOGIC TÌM KIẾM (Search In-Memory) ---
      if (!string.IsNullOrEmpty(keyword))
      {
         // Vì Mock dùng JobPostID/BusinessID, mình sẽ search theo Title và Description
         // (hoặc BusinessID nếu bạn chưa có BusinessName trong Entity JobPost)
         query = query.Where(j =>
             j.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
             (j.Description != null && j.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
         );
      }

      if (locationId.HasValue)
      {
         query = query.Where(j => j.LocationID == locationId);
      }

      // --- TRUYỀN DỮ LIỆU RA VIEW ---
      ViewBag.Keyword = keyword;
      ViewBag.CurrentLocation = locationId;

      // Lấy danh sách Location duy nhất từ dữ liệu Mock để hiển thị dropdown filter ở trang Search
      ViewBag.Locations = allJobs.Select(j => j.Location)
                                 .GroupBy(l => l.LocationID)
                                 .Select(g => g.First())
                                 .ToList();

      // Sắp xếp theo ID mới nhất và trả về List
      var results = query.OrderByDescending(j => j.JobPostID).ToList();

      return View(results); // Trả về Search.cshtml với dữ liệu Mock
   }


   public IActionResult GetJobByID(string id) // Đổi tham số sang string để khớp với JobPostID (Guid)
   {
      // 1. Lấy dữ liệu từ Mock
      var allJobs = JobMock.GetJobs();

      // Tìm kiếm công việc theo ID (string)
      var job = allJobs.FirstOrDefault(j => j.JobPostID == id);

      if (job == null)
      {
         return NotFound();
      }

      // --- LOGIC TÌM KIẾM CÔNG VIỆC LIÊN QUAN ---
      // Tách từ khóa từ Title (ví dụ: "Backend Developer" -> ["Backend", "Developer"])
      var keywords = job.Title.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                              .Where(k => k.Length > 2)
                              .ToList();

      // Lọc các job khác có cùng từ khóa Title hoặc cùng địa điểm
      var relatedJobs = allJobs
         .Where(j => j.JobPostID != id)
         .Where(j =>
            keywords.Any(k => j.Title.Contains(k, StringComparison.OrdinalIgnoreCase)) ||
            j.LocationID == job.LocationID
         )
         .OrderByDescending(j => j.JobPostID)
         .Take(3)
         .ToList();

      // 2. Đưa vào ViewBag để hiển thị ở Sidebar hoặc bên dưới
      ViewBag.RelatedJobs = relatedJobs;

      // Trả về View chi tiết công việc
      return View(job);
   }
}
