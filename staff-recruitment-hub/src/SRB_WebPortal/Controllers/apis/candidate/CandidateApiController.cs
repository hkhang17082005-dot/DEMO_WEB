using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;
using SRB_WebPortal.Shared;

namespace SRB_WebPortal.Controllers.apis.candidate
{
   [Route("api/CandidateApi")]
   [ApiController]
   // [Authorize(Roles = "Candidate")] // Mở comment nếu dùng JWT/Cookie auth
   public class CandidateApiController : ControllerBase
   {
      private readonly DatabaseContext _context;

      public CandidateApiController(DatabaseContext context)
      {
         _context = context;
      }

      [HttpGet("my-applications")]
      public async Task<IActionResult> GetMyApplications([FromQuery] string? lastId = null, [FromQuery] int size = 5, [FromQuery] int? status = null)
      {
         try
         {
            // 1. Lấy ID của User đang đăng nhập (tùy vào cách bạn config Auth)
            var userId = User.FindFirst("Id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // MOCK DATA (Nếu chưa có tính năng đăng nhập, hãy hardcode 1 UserID có sẵn trong DB để test)
            // userId = "USER_ID_CUA_BAN_TRONG_DB";

            if (string.IsNullOrEmpty(userId))
            {
               return Unauthorized(new BaseResponse<string> { IsSuccess = false, Message = "Chưa đăng nhập" });
            }

            // 2. Query cơ sở dữ liệu
            var query = _context.JobApplications
                .Include(a => a.JobPost)
                    .ThenInclude(jp => jp.Business) // Lấy thông tin công ty
                .Where(a => a.UserID == userId)
                .AsQueryable();

            // Lọc theo trạng thái nếu có
            if (status.HasValue)
            {
               query = query.Where(a => (int)a.Status == status.Value);
            }

            // Sắp xếp mới nhất lên đầu
            query = query.OrderByDescending(a => a.AppliedAt);

            // Lấy tất cả list để phân trang (Logic giống bên Business)
            var allApplications = await query.ToListAsync();

            IEnumerable<JobApplication> pagedApplications;

            if (!string.IsNullOrEmpty(lastId))
            {
               pagedApplications = allApplications
                   .SkipWhile(a => a.ApplicationID != lastId)
                   .Skip(1)
                   .Take(size);
            }
            else
            {
               pagedApplications = allApplications.Take(size);
            }

            // 3. Map từ Entity (DB) sang DTO (JSON)
            var resultData = pagedApplications.Select(a => new JobApplicationDTO
            {
               ApplicationID = a.ApplicationID,
               JobPostID = a.JobPostID,
               JobTitle = a.JobPost?.Title ?? "Chưa cập nhật",
               BusinessName = a.JobPost?.Business?.BusinessName ?? "Chưa cập nhật",
               BusinessLogoUrl = a.JobPost?.Business?.LogoUrl,
               Status = (int)a.Status, // Ép kiểu Enum sang int (0, 1, 2,...)
               AppliedAt = a.AppliedAt,
               UpdatedAt = a.UpdatedAt,
               CVPath = a.CVPath
            }).ToList();

            // 4. Trả về format JSON BaseResponse chuẩn của dự án
            return Ok(new BaseResponse<List<JobApplicationDTO>>
            {
               IsSuccess = true,
               Data = resultData,
               Message = "Lấy danh sách thành công"
            });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new BaseResponse<string>
            {
               IsSuccess = false,
               Message = "Lỗi server: " + ex.Message
            });
         }
      }
      // 1. API Lấy thông tin hồ sơ
      [HttpGet("profile")]
      public async Task<IActionResult> GetProfile()
      {
         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("Id")?.Value;
         if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse<string> { IsSuccess = false, Message = "Chưa đăng nhập" });

         var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);

         if (profile == null)
         {
            return NotFound(new BaseResponse<string> { IsSuccess = false, Message = "Không tìm thấy hồ sơ" });
         }

         var dto = new CandidateProfileDTO
         {
            FullName = profile.FullName,
            Email = profile.Email,
            PhoneNumber = profile.PhoneNumber,
            Summary = profile.Summary,
            JobTitle = profile.JobTitle,
            ExpectedSalary = profile.ExpectedSalary,
            Experience = profile.Experience,
            WorkType = profile.WorkType,
            Skills = profile.Skills
         };

         return Ok(new BaseResponse<CandidateProfileDTO> { IsSuccess = true, Data = dto, Message = "Thành công" });
      }

      // 2. API Cập nhật hồ sơ
      [HttpPost("profile")]
      public async Task<IActionResult> UpdateProfile([FromBody] CandidateProfileDTO request)
      {
         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("Id")?.Value;
         if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse<string> { IsSuccess = false, Message = "Chưa đăng nhập" });

         var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserID == userId);

         if (profile == null)
         {
            profile = new UserProfile
            {
               UserID = userId,
               Email = request.Email,
               FullName = request.FullName
            };
            _context.UserProfiles.Add(profile);
         }

         // Map toàn bộ dữ liệu mới
         profile.FullName = request.FullName;
         profile.PhoneNumber = request.PhoneNumber;
         profile.Summary = request.Summary;
         profile.JobTitle = request.JobTitle;
         profile.ExpectedSalary = request.ExpectedSalary;
         profile.Experience = request.Experience;
         profile.WorkType = request.WorkType;
         profile.Skills = request.Skills;

         await _context.SaveChangesAsync();

         return Ok(new BaseResponse<string> { IsSuccess = true, Message = "Cập nhật hồ sơ thành công!" });
      }
   }
}




// using Microsoft.AspNetCore.Mvc;
// using SRB_WebPortal.Shared;
// using System.Linq; // Thêm thư viện này để dùng hàm lọc Where

// namespace SRB_WebPortal.Controllers.apis.candidate
// {
//     [Route("api/CandidateApi")]
//     [ApiController]
//     public class CandidateApiController : ControllerBase
//     {
//         // THÊM THAM SỐ: [FromQuery] int? status
//         [HttpGet("my-applications")]
//         public IActionResult GetMyApplications([FromQuery] int? status = null)
//         {
//             // 1. TẠO DỮ LIỆU GIẢ (MOCK DATA)
//             var mockData = new List<JobApplicationDTO>
//             {
//                 new JobApplicationDTO
//                 {
//                     ApplicationID = "APP001",
//                     JobPostID = "JOB001",
//                     JobTitle = "Chuyên Viên Phát Triển Phần Mềm (C# / ASP.NET)",
//                     BusinessName = "Tập đoàn FPT Software",
//                     BusinessLogoUrl = "https://ui-avatars.com/api/?name=FPT&background=fff&color=F97316",
//                     Status = 2, // 2: Hẹn phỏng vấn
//                     AppliedAt = DateTime.Now.AddDays(-2),
//                     UpdatedAt = DateTime.Now,
//                     CVPath = "HoangKhang_Backend.pdf"
//                 },
//                 new JobApplicationDTO
//                 {
//                     ApplicationID = "APP002",
//                     JobPostID = "JOB002",
//                     JobTitle = "Backend Engineer (Golang/C#)",
//                     BusinessName = "Công ty Cổ phần VNG",
//                     BusinessLogoUrl = "https://ui-avatars.com/api/?name=VNG&background=fff&color=000",
//                     Status = 1, // 1: NTD đã xem CV
//                     AppliedAt = DateTime.Now.AddDays(-5),
//                     UpdatedAt = DateTime.Now,
//                     CVPath = "HoangKhang_Backend.pdf"
//                 },
//                 new JobApplicationDTO
//                 {
//                     ApplicationID = "APP003",
//                     JobPostID = "JOB003",
//                     JobTitle = "Senior Backend Developer (.NET Core)",
//                     BusinessName = "Tech Corp Vietnam",
//                     BusinessLogoUrl = "https://ui-avatars.com/api/?name=Tech+Corp&background=fff&color=000",
//                     Status = 0, // 0: Đã nộp
//                     AppliedAt = DateTime.Now,
//                     UpdatedAt = DateTime.Now,
//                     CVPath = "CV_ChuyenNghiep.pdf"
//                 }
//             };

//             // 2. LỌC DỮ LIỆU THEO TAB (STATUS)
//             // Nếu status có giá trị (tức là không phải tab "Tất cả")
//             if (status.HasValue)
//             {
//                 mockData = mockData.Where(x => x.Status == status.Value).ToList();
//             }

//             // 3. Trả về kết quả đã lọc
//             return Ok(new BaseResponse<List<JobApplicationDTO>>
//             {
//                 IsSuccess = true,
//                 Data = mockData,
//                 Message = "Lấy dữ liệu mẫu thành công"
//             });
//         }
//     }
// }
