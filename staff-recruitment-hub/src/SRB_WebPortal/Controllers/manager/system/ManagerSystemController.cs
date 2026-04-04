using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Data;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.Job;
using SRB_WebPortal.Controllers.apis.user;
using SRB_WebPortal.Models;

namespace SRB_WebPortal.Controllers.manager.system;

[Route("manager/system/ManagerSystem")]
[AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER, Roles.SUPPORT })]
//[Authorize(Roles = "Admin")]
public class ManagerSystemController(IUserService userService, IJobPostService jobPostService, DatabaseContext dbContext) : Controller
{
   private readonly IUserService _userService = userService;
   private readonly IJobPostService _jobPostService = jobPostService;
   private readonly DatabaseContext _dbContext = dbContext;

   private double CalculateGrowth(int currentCount, int lastMonthCount)
   {
      // if(lastMonthCount == 0) return 100; // Trường hợp tháng trước không có gì, tăng trưởng 100%
      if (lastMonthCount == 0) return currentCount > 0 ? 100 : 0; // Nếu tháng trước không có gì, chỉ tính là tăng trưởng nếu tháng này có gì đó

      var growth = ((double)(currentCount - lastMonthCount) / lastMonthCount) * 100;
      return Math.Round(growth, 1);//return Math.Round(growth, 1); // Làm tròn đến 1 chữ số thập phân
   }
   public async Task<IActionResult> Index()
   {
      //user 
      var users = await _userService.GetCandidatesAsync(); // Lấy tất cả candidates, có thể filter theo role nếu cần


      var now = DateTime.UtcNow;
      var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
      var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);

      var allCandidates = await _dbContext.Users
        .Where(u => u.UserRoles.Any(ur => ur.RoleID == 50))
        .ToListAsync();

      int candidatesThisMonth = allCandidates.Count(u => u.CreatedAt >= firstDayThisMonth);
      int candidatesLastMonth = allCandidates.Count(u => u.CreatedAt >= firstDayLastMonth && u.CreatedAt < firstDayThisMonth);

      //Job post
      var jobPosts = await _jobPostService.GetAllAsync();
      int jobsThisMonth = jobPosts.Count(j => j.CreatedAt >= firstDayThisMonth);
      int jobsLastMonth = jobPosts.Count(j => j.CreatedAt >= firstDayLastMonth && j.CreatedAt < firstDayThisMonth);

      //Business
      var businesses = await _dbContext.Businesses.ToListAsync();
      int busThisMonth = businesses.Count(b => b.CreatedAt >= firstDayThisMonth);
      int busLastMonth = businesses.Count(b => b.CreatedAt >= firstDayLastMonth && b.CreatedAt < firstDayThisMonth);
      //aplication
      var applications = await _dbContext.JobApplications.ToListAsync();
      int appsThisMonth = applications.Count(a => a.AppliedAt >= firstDayThisMonth); // Giả sử tên field là AppliedAt
      int appsLastMonth = applications.Count(a => a.AppliedAt >= firstDayLastMonth && a.AppliedAt < firstDayThisMonth);
      var dashboard = new DashboardViewModel
      {
         TotalCandidates = users.Count, // Giả sử tất cả users là candidates, có thể filter theo role
         TotalJobPosts = jobPosts.Count,
         TotalApplications = applications.Count,
         TotalBusinesses = businesses.Count,
         CandidatesGrowth = CalculateGrowth(candidatesThisMonth, candidatesLastMonth),
         JobPostsGrowth = CalculateGrowth(jobsThisMonth, jobsLastMonth),
         ApplicationsGrowth = CalculateGrowth(appsThisMonth, appsLastMonth),
         BusinessesGrowth = CalculateGrowth(busThisMonth, busLastMonth)
      };

      return View(dashboard);
   }

   [HttpGet("QuanLyDoanhNghiep")]
   public async Task<IActionResult> QuanLyDoanhNghiep()
   {
      var businesses = await _dbContext.Businesses.ToListAsync();
      return View("QuanLyDoanhNghiep/Index", businesses);
   }

   [HttpGet("QuanLyNguoiDung")]
   public async Task<IActionResult> QuanLyNguoiDung()
   {
      var users = await _userService.GetAllUsersAsync();
      return View("QuanLyNguoiDung/Index", users);
   }


   [HttpGet("QuanLyTinTuyenDung")]
   public async Task<IActionResult> QuanLyTinTuyenDung()
   {
      var posts = await _jobPostService.GetAllAsync();
      return View("QuanLyTinTuyenDung/Index", posts);
   }
}
