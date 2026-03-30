using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Data;

public static class JobMock
{
   public static List<JobPost> GetJobs()
   {
      return
      [
         new JobPost
         {
            JobPostID = "019d36ff-1c1d-74e3-8be4-dbee467ed0b9",
            Title = "Backend Developer .NET",
            BusinessID = "019d3701-6c0b-7c78-8aed-c362b3897e31",
            JobType = JobType.FullTime,
            Description = "Cần tuyển 10 lập trình viên\nPhát triển hệ thống Backend, xây dựng API",
            Requirements = "- C#\n- SQL Server\n- .NET Core\nCó từ 2 năm kinh nghiệm trở lên\nLàm việc từ T2 - T6 nghỉ CN",
            SalaryRange = "15 - 25 triệu",
            Benefits = "- Lương tháng 13\n- Thưởng KPI\n- Du lịch",
            LocationID = 1,
         },

         new JobPost
         {
            JobPostID = "019d36ff-1c1d-74e3-8be4-dbee467ed0c9",
            Title = "Backend Developer .NET",
            BusinessID = "019d3701-6c0b-7c78-8aed-c362b3897e31",
            JobType = JobType.FullTime,
            Description = "Cần tuyển 10 lập trình viên\nPhát triển hệ thống Backend, xây dựng API",
            Requirements = "- C#\n- SQL Server\n- .NET Core\nCó từ 2 năm kinh nghiệm trở lên\nLàm việc từ T2 - T6 nghỉ CN",
            SalaryRange = "15 - 25 triệu",
            Benefits = "- Lương tháng 13\n- Thưởng KPI\n- Du lịch",
            LocationID = 1,
         },

         new JobPost
         {
            JobPostID = "019d36ff-2b2d-74e3-9ce5-ebee467ed0c0",
            Title = "Frontend Developer React",
            BusinessID = "019d3701-7d1c-8d89-9bfe-d473c4908f42",
            JobType = JobType.FullTime,
            Description = "Phát triển giao diện web bằng ReactJS.",
            Requirements = "- ReactJS\n- HTML/CSS\n- JavaScript\nKinh nghiệm 1 năm",
            SalaryRange = "12 - 20 triệu",
            Benefits = "- Thưởng KPI\n- Snack miễn phí\n- Môi trường trẻ",
            LocationID = 2,
         },

         new JobPost
         {
            JobPostID = "019d36ff-3c3e-74e3-ad4f-fbee467ed0d1",
            Title = "UI/UX Designer",
            BusinessID = "019d3701-8e2d-9e90-0c0f-e584d5019g53",
            JobType = JobType.FullTime,
            Description = "Thiết kế giao diện web/app.",
            Requirements = "- Figma\n- Photoshop\n- UI/UX\nLinh hoạt thời gian",
            SalaryRange = "10 - 18 triệu",
            Benefits = "- Thưởng dự án\n- Môi trường sáng tạo",
            LocationID = 3,
         },

         new JobPost
         {
            JobPostID = "019d36ff-4d4f-74e3-be5g-gbee467ed0e2",
            Title = "Kế toán tổng hợp",
            BusinessID = "019d3701-9f3e-0f01-1d1g-f695e6120h64",
            JobType = JobType.FullTime,
            Description = "Quản lý sổ sách kế toán.",
            Requirements = "- Excel\n- Kế toán\nKinh nghiệm 2 năm, ưu tiên Nữ",
            SalaryRange = "8 - 15 triệu",
            Benefits = "- BHXH đầy đủ\n- Công việc ổn định",
            LocationID = 4,
         },

         new JobPost
         {
            JobPostID = "019d36ff-5e5g-74e3-cf6h-hbee467ed0f3",
            Title = "Mobile App Developer (Flutter/RN)",
            BusinessID = "019d3701-af4f-1f12-2e2h-g706f7231i75",
            JobType = JobType.FullTime,
            Description = "Phát triển ứng dụng di động đa nền tảng.\nTối ưu hiệu năng và trải nghiệm người dùng.",
            Requirements = "- Flutter hoặc React Native\n- Có kiến thức về Mobile UI/UX\n- Kinh nghiệm 2 năm",
            SalaryRange = "18 - 35 triệu",
            Benefits = "- Cấp MacBook làm việc\n- Thưởng dự án quý\n- TeamBuilding hàng quý",
            LocationID = 5,
         },

         new JobPost
         {
            JobPostID = "019d36ff-6f6h-74e3-dg7i-ibee467ed104",
            Title = "Senior Devops Engineer",
            BusinessID = "019d3701-bg5g-2g23-3f3i-h817g8342j86",
            JobType = JobType.Remote,
            Description = "Thiết kế và triển khai hạ tầng Cloud (AWS/Azure).\nQuản lý hệ thống CI/CD cho dự án quốc tế.",
            Requirements = "- Docker, Kubernetes\n- Terraform, Ansible\n- Tiếng Anh giao tiếp tốt",
            SalaryRange = "40 - 60 triệu",
            Benefits = "- Làm việc 100% tại nhà\n- Bảo hiểm sức khỏe quốc tế\n- Thưởng cổ phiếu",
            LocationID = 1,
         },

         new JobPost
         {
            JobPostID = "019d36ff-7g7i-74e3-eh8j-jbee467ed115",
            Title = "Nhân viên TeleSale Bất động sản",
            BusinessID = "019d3701-ch6h-3h34-4g4j-i928h9453k97",
            JobType = JobType.PartTime,
            Description = "Gọi điện tư vấn khách hàng theo danh sách có sẵn.\nHẹn khách tham quan dự án.",
            Requirements = "- Giọng nói dễ nghe\n- Không yêu cầu kinh nghiệm\n- Chịu khó, kiên trì",
            SalaryRange = "5 - 8 triệu + Hoa hồng",
            Benefits = "- Đào tạo kỹ năng chốt sale\n- Môi trường năng động\n- Cơ hội thăng tiến",
            LocationID = 2,
         },

         new JobPost
         {
            JobPostID = "019d36ff-8h8j-74e3-fi9k-kbee467ed126",
            Title = "Thực tập sinh Marketing",
            BusinessID = "019d3701-di7i-4i45-5h5k-j039i0564l08",
            JobType = JobType.Internship,
            Description = "Hỗ trợ quản lý FanPage, sáng tạo nội dung TikTok.\nTham gia lên ý tưởng cho các chiến dịch truyền thông.",
            Requirements = "- Sinh viên năm 3, 4 chuyên ngành Marketing\n- Biết sử dụng Canva, CapCut cơ bản",
            SalaryRange = "3 - 5 triệu",
            Benefits = "- Dấu mộc thực tập\n- Cơ hội trở thành nhân viên chính thức\n- Snack & Cafe free",
            LocationID = 3,
         },

         new JobPost
         {
            JobPostID = "019d36ff-9i9k-74e3-gj0l-lbee467ed137",
            Title = "Data Analyst",
            BusinessID = "019d3701-ej8j-5j56-6i6l-k140j1675m19",
            JobType = JobType.Hybrid,
            Description = "Phân tích dữ liệu kinh doanh.\nXây dựng báo cáo Dashboard trên PowerBI/Tableau.",
            Requirements = "- SQL, Python/R\n- Kỹ năng tư duy logic tốt\n- Kinh nghiệm 1-2 năm",
            SalaryRange = "20 - 30 triệu",
            Benefits = "- Remote 2 ngày/tuần\n- Thưởng tháng lương 14\n- Khóa học chuyên môn miễn phí",
            LocationID = 4,
         },

         new JobPost
         {
            JobPostID = "019d36ff-0j0l-74e3-hk1m-mbee467ed148",
            Title = "Graphic Designer 2D",
            BusinessID = "019d3701-fk9k-6k67-7j7m-l251k2786n20",
            JobType = JobType.Freelance,
            Description = "Thiết kế bộ nhận diện thương hiệu cho dự án.\nLàm việc theo từng giai đoạn dự án.",
            Requirements = "- Sử dụng thành thạo AI, Photoshop\n- Có Portfolio ấn tượng\n- Đúng tiến độ (Deadline)",
            SalaryRange = "Theo dự án",
            Benefits = "- Thời gian tự do\n- Làm việc từ xa\n- Cộng tác lâu dài",
            LocationID = 5,
         },

         new JobPost
         {
            JobPostID = "019d36ff-1k1m-74e3-il2n-nbee467ed159",
            Title = "Quality Control (QC/Tester)",
            BusinessID = "019d3701-gl0l-7l78-8k8n-m362l3897o31",
            JobType = JobType.FullTime,
            Description = "Kiểm thử phần mềm Web/App.\nViết Testcase và báo cáo lỗi (Bug report).",
            Requirements = "- Hiểu biết về quy trình kiểm thử\n- Cẩn thận, tỉ mỉ\n- Ưu tiên biết Automation Test",
            SalaryRange = "15 - 22 triệu",
            Benefits = "- Thưởng các ngày lễ tết\n- Khám sức khỏe định kỳ\n- Môi trường chuyên nghiệp",
            LocationID = 1,
         },

         new JobPost
         {
            JobPostID = "019d36ff-2l2n-74e3-jm3o-obee467ed160",
            Title = "Nhân viên Hành chính Nhân sự",
            BusinessID = "019d3701-hm1m-8m89-9l9o-n473m4908p42",
            JobType = JobType.FullTime,
            Description = "Quản lý hồ sơ nhân viên, chấm công.\nHỗ trợ công tác tuyển dụng và đào tạo.",
            Requirements = "- Tốt nghiệp đại học chuyên ngành liên quan\n- Kỹ năng giao tiếp tốt\n- Thành thạo tin học văn phòng",
            SalaryRange = "10 - 15 triệu",
            Benefits = "- BHXH theo luật định\n- Nghỉ phép 12 ngày/năm\n- Ăn trưa tại công ty",
            LocationID = 2,
         },

         new JobPost
         {
            JobPostID = "019d36ff-3m3o-74e3-kn4p-pbee467ed171",
            Title = "Content Creator (Social Media)",
            BusinessID = "019d3701-in2n-9n90-0m0p-o584n5019q53",
            JobType = JobType.FullTime,
            Description = "Viết bài PR, kịch bản video cho Facebook, Instagram.\nTheo dõi xu hướng (trending) để triển khai nội dung.",
            Requirements = "- Khả năng viết tốt, sáng tạo\n- Am hiểu mạng xã hội\n- Biết chụp ảnh cơ bản là điểm cộng",
            SalaryRange = "12 - 18 triệu",
            Benefits = "- Môi trường GenZ năng động\n- Đi du lịch 2 lần/năm\n- Thưởng sáng tạo",
            LocationID = 3,
         },

         new JobPost
         {
            JobPostID = "019d36ff-4n4p-74e3-lo5q-qbee467ed182",
            Title = "Kỹ sư Hệ thống Embedded",
            BusinessID = "019d3701-jo3o-0o01-1n1q-p695o6120r64",
            JobType = JobType.FullTime,
            Description = "Lập trình vi điều khiển, hệ thống nhúng.\nTham gia thiết kế mạch và xử lý tín hiệu.",
            Requirements = "- C/C++, hiểu về MCU (STM32, ESP32)\n- Đọc hiểu sơ đồ nguyên lý mạch điện\n- Kinh nghiệm 2 năm",
            SalaryRange = "25 - 45 triệu",
            Benefits = "- Hỗ trợ tiền ăn trưa, gửi xe\n- Thưởng cuối năm hấp dẫn\n- Làm việc với chuyên gia nước ngoài",
            LocationID = 4,
         }
      ];
   }

   public static List<JobPost> GetMockJobPosts()
   {
      var allJobs = GetJobs();

      return [.. allJobs.OrderBy(x => Guid.NewGuid()).Take(3)];
   }

   // Class tạm để giả lập dữ liệu CV (Sau này sẽ thay bằng Entity DB)
   public class MockApplication
   {
      public required string ApplicationId { get; set; }
      public required string JobPostID { get; set; }
      public required string CandidateName { get; set; }
      public required string Email { get; set; }
      public DateTime AppliedDate { get; set; }
      public required string Status { get; set; }
      public required string StatusColor { get; set; }
   }

   // Hàm sinh ra danh sách CV dựa vào mã chiến dịch (jobId)
   public static List<MockApplication> GetMockApplications(string jobId)
   {
      return
         [
            new MockApplication {
            ApplicationId = "CV_001", JobPostID = jobId, CandidateName = "Nguyễn Văn A", Email = "nva@gmail.com",
            AppliedDate = DateTime.UtcNow.AddMinutes(-30), Status = "Mới tiếp nhận", StatusColor = "bg-yellow-100 text-yellow-800 border-yellow-200"
         },
         new MockApplication {
            ApplicationId = "CV_002", JobPostID = jobId, CandidateName = "Trần Thị B", Email = "tranthib@gmail.com",
            AppliedDate = DateTime.UtcNow.AddHours(-5), Status = "Đang phỏng vấn", StatusColor = "bg-blue-100 text-blue-800 border-blue-200"
         },
         new MockApplication {
            ApplicationId = "CV_003", JobPostID = jobId, CandidateName = "Lê Hoàng C", Email = "lehoangc@gmail.com",
            AppliedDate = DateTime.UtcNow.AddDays(-1), Status = "Đã loại", StatusColor = "bg-red-100 text-red-800 border-red-200"
         }
         ];
   }
}
