using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_ViewModel.Data;

namespace SRB_WebPortal.Controllers.routes;

[AuthGuard(Roles = new[] {
      Roles.BUSINESS_MANAGER,
      Roles.HIRING_MANAGER,
      Roles.HR_MANAGER,
      Roles.INTERVIEWER,
      Roles.RECRUITER
   })]
public class BusinessController : Controller
{
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
   public IActionResult PostJob()
   {
      return View();
   }

   public IActionResult MyJobs()
   {
      // Trang quản lý danh sách tin tuyển dụng
      return View();
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   }

   public IActionResult RegisterBusiness()
   {
      return View();
   }
}
