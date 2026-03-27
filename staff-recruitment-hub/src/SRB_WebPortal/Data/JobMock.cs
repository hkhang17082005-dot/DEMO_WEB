using System;
using System.Collections.Generic;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Data;

public static class JobMock
{
    // Hàm này sẽ sinh ra 3 tin tuyển dụng giả lập
    public static List<JobPost> GetMockJobPosts()
    {
        return new List<JobPost>
        {
            new JobPost
            {
                JobPostID = Guid.NewGuid().ToString(),
                Title = "Lập trình viên Backend .NET (Middle/Senior)",
                Description = "Phát triển các API với ASP.NET Core, làm việc với SQL Server...",
                SalaryRange = "15,000,000 - 25,000,000 VND",
                Location = "TP. Hồ Chí Minh",
                IsActive = true,
                BusinessID = "BUSS_TEMP_01",
                CreatedByID = "USER_TEMP_01",
                CreatedAt = DateTime.UtcNow.AddDays(-2) // Đăng cách đây 2 ngày
            },
            new JobPost
            {
                JobPostID = Guid.NewGuid().ToString(),
                Title = "Chuyên viên Digital Marketing",
                Description = "Lên kế hoạch chạy Ads, quản lý hệ thống Fanpage công ty...",
                SalaryRange = "10,000,000 - 15,000,000 VND",
                Location = "Hà Nội",
                IsActive = true,
                BusinessID = "BUSS_TEMP_01",
                CreatedByID = "USER_TEMP_01",
                CreatedAt = DateTime.UtcNow.AddDays(-5) // Đăng cách đây 5 ngày
            },
            new JobPost
            {
                JobPostID = Guid.NewGuid().ToString(),
                Title = "Thực tập sinh Business Analyst (BA)",
                Description = "Hỗ trợ lấy yêu cầu khách hàng, viết tài liệu phân tích hệ thống...",
                SalaryRange = "Thỏa thuận",
                Location = "Đà Nẵng",
                IsActive = false, // Tin này đã đóng
                BusinessID = "BUSS_TEMP_01",
                CreatedByID = "USER_TEMP_01",
                CreatedAt = DateTime.UtcNow.AddDays(-15) // Đăng cách đây 15 ngày
            }
        };
    }
}