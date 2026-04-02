using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Shared;
using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.post;
using SRB_WebPortal.Consts;

namespace SRB_WebPortal.Controllers.routes;

[AuthGuard(Roles = new[] { Roles.CANDIDATE })]
public class CandidateController(IShareRepository shareRepository) : Controller
{
   private readonly IShareRepository _shareRepository = shareRepository;

   public async Task<IActionResult> Index()
   {
      if (HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload tokenInfo)
      {
         var userID = tokenInfo.User.UserID;

         var dashboardData = await _shareRepository.GetDashboardStatsAsync(userID);

         return View(dashboardData);
      }

      return RedirectToAction("Login", "Auth");
   }

   // Trang Quản lý Hồ sơ & CV
   public IActionResult Profile()
   {
      return View();
   }

   public async Task<List<MyApplicationDTO>> GetMyApplications(string userID)
   {
      return await _shareRepository.GetUserApplicationsAsync(userID);
   }

   public async Task<IActionResult> MyApplications()
   {
      if (HttpContext.Items[ServerKey.CONTEXT_ITEM_TOKEN_INFO] is TokenPayload tokenInfo)
      {
         var userID = tokenInfo.User.UserID;

         var applications = await _shareRepository.GetUserApplicationsAsync(userID);

         return View(applications);
      }

      return RedirectToAction("Login", "Auth");
   }


   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

      return View(new ErrorViewModel(requestId, 500, "Đã có lỗi xảy ra"));
   }
}
