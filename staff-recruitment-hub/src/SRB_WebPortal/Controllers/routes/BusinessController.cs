using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Models;
using SRB_ViewModel.Entities;

using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;

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

   public async Task<IActionResult> EditJobPost([FromQuery] string jobPostId)
   {
      var foundJobPost = await _shareRepository.GetJobPost(jobPostId);

      if (foundJobPost is null)
      {
         return NotFound();
      }

      var resPost = new JobPostDTO
      {
         JobPostID = jobPostId,
         Title = foundJobPost.Title,
         BusinessID = foundJobPost.BusinessID,
         BusinessName = foundJobPost.Business.BusinessName,
         BusinessLogoURL = foundJobPost.Business.LogoUrl,
         JobType = foundJobPost.JobType,
         SalaryRange = foundJobPost.SalaryRange,
         Description = foundJobPost.Description,
         Requirements = foundJobPost.Requirements,
         Benefits = foundJobPost.Benefits,
         LocationID = foundJobPost.LocationID,
         Address = foundJobPost.Address,
         ExpiryDate = foundJobPost.ExpiryDate,
         CreatedAt = foundJobPost.CreatedAt
      };

      ViewBag.Locations = await _shareRepository.GetLocations();

      return View(resPost);
   }

   public async Task<IActionResult> DetailJobPost([FromQuery] string jobPostId)
   {
      var foundJobPost = await _shareRepository.GetJobPost(jobPostId);

      if (foundJobPost is null)
      {
         return NotFound();
      }

      var resPost = new JobPostDTO
      {
         JobPostID = jobPostId,
         Title = foundJobPost.Title,
         BusinessID = foundJobPost.BusinessID,
         BusinessName = foundJobPost.Business.BusinessName,
         BusinessLogoURL = foundJobPost.Business.LogoUrl,
         JobType = foundJobPost.JobType,
         SalaryRange = foundJobPost.SalaryRange,
         Description = foundJobPost.Description,
         Requirements = foundJobPost.Requirements,
         Benefits = foundJobPost.Benefits,
         LocationID = foundJobPost.LocationID,
         Address = foundJobPost.Address,
         ExpiryDate = foundJobPost.ExpiryDate,
         CreatedAt = foundJobPost.CreatedAt
      };

      ViewBag.Locations = await _shareRepository.GetLocations();

      return View(resPost);
   }

   public async Task<IActionResult> CVList([FromQuery] string jobPostId)
   {
      if (string.IsNullOrWhiteSpace(jobPostId))
      {
         return RedirectToAction("MyJobPost");
      }

      var cvList = await _shareRepository.GetPendingJobPostsAsync(jobPostId);

      var jobPost = await _shareRepository.GetJobPost(jobPostId);

      if (jobPost == null)
      {
         TempData["Error"] = "Chiến dịch không tồn tại!";
         return RedirectToAction("MyJobPost");
      }

      ViewBag.JobPostId = jobPostId;
      ViewBag.JobTitle = jobPost.Title;

      return View(cvList);
   }

   public async Task<IActionResult> ReviewCV([FromQuery] string applicationID)
   {
      Console.WriteLine($"NQHxLog: {applicationID}");

      if (string.IsNullOrEmpty(applicationID)) return NotFound();

      var cvDetail = await _shareRepository.GetApplicationDetailAsync(applicationID);

      if (cvDetail == null)
      {
         TempData["Error"] = "Không tìm thấy hồ sơ ứng viên!";

         return RedirectToAction("MyJobPost");
      }

      return View(cvDetail);
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
