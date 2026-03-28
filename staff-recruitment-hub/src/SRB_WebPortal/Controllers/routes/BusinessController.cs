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
      return View(mockJobs);
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
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
   }

   public IActionResult RegisterBusiness()
   {
      return View();
   }
}
