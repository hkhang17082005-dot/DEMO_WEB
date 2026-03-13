using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Controllers.routes
{
   //[Subdomain("manager")] // Chỉ cho phép truy cập từ manager.localhost
   //[AuthGuard(Roles = new[] { "BUSINESS_MANAGER", "HIRING_MANAGER" })] // Bảo mật
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
   }
}
