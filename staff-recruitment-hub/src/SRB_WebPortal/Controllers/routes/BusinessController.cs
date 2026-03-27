using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel.Data;
using SRB_WebPortal.Models;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_ViewModel;
using SRB_ViewModel.Entities;

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
   private readonly DatabaseContext _context;

   public BusinessController(DatabaseContext context)
   {
      _context = context;
   }
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

   [HttpPost]
   public async Task<IActionResult> PostJob(CreateJobViewModel model)
   {
      if (!ModelState.IsValid)
      {
         return View(model);
      }

      // Xử lý ghép mức lương từ 2 ô Min và Max thành 1 chuỗi "SalaryRange"
      string salaryRange = "Thỏa thuận";
      if (model.MinSalary.HasValue || model.MaxSalary.HasValue)
      {
         salaryRange = $"{model.MinSalary:N0} - {model.MaxSalary:N0} VND";
      }

      // Tạo Entity mới để chuẩn bị lưu xuống SQL
      var newJob = new JobPost
      {
         JobPostID = Guid.NewGuid().ToString(), // Tự phát sinh mã ngẫu nhiên
         Title = model.Title,
         Description = model.Description,
         Location = model.Location,
         SalaryRange = salaryRange,             // Lưu chuỗi lương vừa ghép
         IsActive = true,                       // Mặc định cho tin hiển thị luôn
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow,

         // TẠM THỜI GẮN CỨNG ID CHO ĐẾN KHI LÀM LOGIN:
         // Vì ID của bạn là string, nên mình gắn 1 chuỗi tạm.
         BusinessID = "BUSS_TEMP_01",
         CreatedByID = "USER_TEMP_01"
      };

      // Thêm vào DbContext và Lưu
      _context.JobPosts.Add(newJob);
      await _context.SaveChangesAsync();

      // Lưu thành công -> Chuyển hướng về trang danh sách
      return RedirectToAction("MyJobs");
   }


   public IActionResult MyJobs()
   {
      var mockJobs = SRB_WebPortal.Data.JobMock.GetMockJobPosts();
      // Trang quản lý danh sách tin tuyển dụng
      return View();
   }

   public IActionResult CVList(int jobId = 1)
   {
      // Trang xem danh sách những người đã nộp vào 1 Job
      return View();
   }

   public IActionResult ReviewCV(int id = 1)
   {
      // Trang chi tiết chia đôi màn hình để duyệt 1 CV
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
