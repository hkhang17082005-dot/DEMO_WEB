using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.apis.Job;

public class JobPostsController(IJobPostService service) : Controller
{
   private readonly IJobPostService _service = service;

   public async Task<IActionResult> Index()
   {
      try
      {
         var posts = await _service.GetAllAsync();

         return View("~/Views/ManagerSystem/QuanLyTinTuyenDung/Index.cshtml", posts);
      }
      catch (Exception ex)
      {
         return Content(ex.Message); // Nó sẽ hiện lỗi trực tiếp lên trình duyệt cho bạn thấy
      }
   }

   [HttpPost]
   public async Task<IActionResult> ToggleApproval(string id)
   {
      var post = await _service.ToggleApprovalAsync(id);
      if (post == null) return NotFound();
      return RedirectToAction("Index");
   }

   [HttpPost]
   public async Task<IActionResult> Delete(string id)
   {
      var result = await _service.DeleteAsync(id);
      if (!result) return NotFound();
      return RedirectToAction("Index");
   }
}

