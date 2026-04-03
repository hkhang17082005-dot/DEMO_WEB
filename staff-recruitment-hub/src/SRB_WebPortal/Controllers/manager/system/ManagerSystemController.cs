using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.Job;
using SRB_WebPortal.Controllers.apis.user;

namespace SRB_WebPortal.Controllers.manager.system;

[Route("manager/system/ManagerSystem")]
[AuthGuard(Roles = new[] { Roles.ADMIN, Roles.SYSTEM_MANAGER, Roles.SUPPORT })]
public class ManagerSystemController(IUserService userService, IJobPostService jobPostService) : Controller
{
   private readonly IUserService _userService = userService;
   private readonly IJobPostService _jobPostService = jobPostService;

   [HttpGet("")]
   public IActionResult Index()
   {
      return View();
   }

   [HttpGet("QuanLyDoanhNghiep")]
   public IActionResult QuanLyDoanhNghiep()
   {
      return View("QuanLyDoanhNghiep/Index");
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
