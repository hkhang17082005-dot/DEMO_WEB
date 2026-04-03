using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;

namespace SRB_WebPortal.Controllers.routes;

public class Create_CVController(DatabaseContext context) : Controller
{
   private readonly DatabaseContext _context = context;

   // ===================== VIEW =====================

   public IActionResult Index()
   {
      return View();
   }

   public IActionResult Create()
   {
      return View("Create/Index");
   }

   public async Task<IActionResult> CV_Management()
   {
      var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (!string.IsNullOrEmpty(userID))
      {
         var user = await _context.Users.Include(u => u.UserProfile)
                                        .FirstOrDefaultAsync(u => u.UserID == userID);
         if (user != null)
         {
            ViewBag.Fullname = user.UserProfile?.FullName;
            ViewBag.IsVerified = true;
         }
      }
      else
      {
         ViewBag.Fullname = "Khách";
         ViewBag.IsVerified = false;
      }

      ViewBag.CVCount = 5;
      ViewBag.OpenToWork = false;
      ViewBag.AllowSearch = false;

      return View("CV_Management/Index");
   }

   public async Task<IActionResult> Preview(int id)
   {
      return View("Preview/Index");
   }
   public async Task<IActionResult> All_CV_Style()
   {
      return View("All_CV_Style/Index");
   }

   public async Task<IActionResult> CVChuyenNghiep()
   {
      return View("CVChuyenNghiep/Index");
   }

   public async Task<IActionResult> Simple_CV()
   {
      return View("Simple_CV/Index");
   }

   public async Task<IActionResult> Modern_CV()
   {
      return View("Modern_CV/Index");
   }
   // ===================== SAVE =====================

   // [HttpPost]
   // public async Task<IActionResult> SaveCV([FromBody] CV model)
   // {
   //    if (string.IsNullOrWhiteSpace(model.Title))
   //       model.Title = "CV chưa đặt tên";

   //    if (string.IsNullOrWhiteSpace(model.UserId))
   //       model.UserId = "USER_DEMO_01";

   //    if (string.IsNullOrWhiteSpace(model.Status))
   //       model.Status = "Đang dùng";

   //    model.CreatedAt = DateTime.Now;

   //    _context.CVs.Add(model);

   //    await _context.SaveChangesAsync();

   //    return Json(new { success = true });
   // }

   // ===================== DELETE =====================

   public async Task<IActionResult> Delete(int id)
   {
      return RedirectToAction("CV_Management");
   }


}
