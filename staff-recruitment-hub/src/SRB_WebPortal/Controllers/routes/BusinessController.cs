using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Data;
using SRB_WebPortal.Models;

using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Shared;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.routes;

[AuthGuard(Roles = new[] {
   Roles.BUSINESS_MANAGER,
   Roles.HIRING_MANAGER,
   Roles.HR_MANAGER,
   Roles.INTERVIEWER,
   Roles.RECRUITER
})]
public class BusinessController(IShareRepository shareRepository) : Controller
{
   private readonly IShareRepository _shareRepository = shareRepository;

   public IActionResult Index()
   {
      // Trang Dashboard tổng quan của Doanh nghiệp
      return View();
   }

   public IActionResult Profile()
   {
      // Trang chỉnh sửa thông tin công ty
      return View();
   }
   public async Task<IActionResult> PostJob()
   {
      var locations = await _shareRepository.GetLocations();

      ViewBag.Locations = locations;

      return View();
   }

   public IActionResult MyJobPost(string? lastPostID = null, int postSize = 10)
   {
      var allJobs = JobMock.GetMockJobPosts();

      IEnumerable<JobPost> pagedJobs;

      if (!string.IsNullOrEmpty(lastPostID))
      {
         pagedJobs = [.. allJobs
            .SkipWhile(j => j.JobPostID != lastPostID)
            .Skip(1)
            .Take(postSize)]; ;
      }
      else
      {
         pagedJobs = allJobs.Take(postSize);
      }

      ViewBag.PostSize = postSize;
      ViewBag.LastID = pagedJobs.LastOrDefault()?.JobPostID;
      ViewBag.HasNext = allJobs.Count > (allJobs.ToList().FindIndex(j => j.JobPostID == ViewBag.LastID) + 1);

      return View(pagedJobs);
   }

   public IActionResult CVList(string jobId)
   {
      if (string.IsNullOrEmpty(jobId))
      {
         return RedirectToAction("MyJobs");
      }

      // 1. Tìm tên chiến dịch để hiển thị lên tiêu đề
      var job = SRB_WebPortal.Data.JobMock.GetMockJobPosts().FirstOrDefault(j => j.JobPostID == jobId);
      ViewBag.JobTitle = job != null ? job.Title : "Chiến dịch không xác định";

      // 2. Lấy danh sách CV nộp cho đúng cái jobId này
      var cvList = SRB_WebPortal.Data.JobMock.GetMockApplications(jobId);

      // 3. Truyền danh sách CV ra ngoài Giao diện
      return View(cvList);
   }

   public IActionResult ReviewCV(int id = 1)
   {
      // Trang chi tiết chia đôi màn hình để duyệt 1 CV
      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

      return View(new ErrorViewModel(requestId, 500, "Đã có lỗi xảy ra"));
   }

   public IActionResult RegisterBusiness()
   {
      return View();
   }
}
