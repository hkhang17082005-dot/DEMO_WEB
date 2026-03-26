using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.routes
{
   [AuthGuard(Roles = new[] { Roles.CANDIDATE })] // Bảo mật: Chỉ Ứng viên mới vào được
   public class CandidateController : Controller
   {
      // 1. Trang Dashboard
      public IActionResult Index()
      {
         return View();
      }

      // 2. Trang Quản lý Hồ sơ & CV
      public IActionResult Profile()
      {
         return View();
      }

      // 3. Trang Lịch sử Ứng tuyển
      public IActionResult MyApplications()
      {
         return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
