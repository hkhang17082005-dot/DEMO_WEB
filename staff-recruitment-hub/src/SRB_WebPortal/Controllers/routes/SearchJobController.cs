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

    public async Task<IActionResult> GetJobByID(int id)
    {
        var job = await _context.Jobs
            .Include(j => j.Location)
            .Include(j => j.JobDetail) // Bao gồm JobDetail để hiển thị thông tin chi tiết
            .FirstOrDefaultAsync(j => j.JobID == id);

        if (job == null)
        {
            return NotFound();
        }
        // Tách từ khóa từ Title của công việc đang xem
        var keywords = job.Title.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Where(k => k.Length > 2)
                                .ToList();

        // Tìm kiếm các job khác có Title chứa từ khóa hoặc cùng địa điểm
        var relatedJobs = await _context.Jobs
            .Include(j => j.Location)
            .Where(j => j.JobID != id) // Loại trừ chính nó
            .Where(j => keywords.Any(k => j.Title.Contains(k)) || j.LocationID == job.LocationID)
            .AsNoTracking()
            .OrderByDescending(j => j.JobID) // Tin mới nhất lên đầu
            .Take(3) // Chỉ lấy 3 tin để hiển thị đẹp giao diện
            .ToListAsync();

        // 3. Đưa vào ViewBag để sang View hiển thị
        ViewBag.RelatedJobs = relatedJobs;

        return View(job); // trả về GetJobById.cshtml
    }
}
