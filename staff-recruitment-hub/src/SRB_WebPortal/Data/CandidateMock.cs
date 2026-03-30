using System;
using System.Collections.Generic;
using System.Linq;

namespace SRB_WebPortal.Data;

public class CandidateMockModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public string[] Skills { get; set; } = Array.Empty<string>();
    public string Avatar { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public static class CandidateMock
{
    // Hàm khởi tạo danh sách ứng viên gốc
    public static List<CandidateMockModel> GetMockCandidates()
    {
        return new List<CandidateMockModel>
        {
            new() { Id = 1, Name = "Nguyễn Văn A", Title = ".NET Developer", Experience = "3 năm", Skills = new[] {"C#", ".NET Core", "SQL Server"}, Avatar = "/images/cv1.jpg", Status = "Đang tìm việc" },
            new() { Id = 2, Name = "Trần Thị B", Title = "Frontend Developer", Experience = "2 năm", Skills = new[] {"ReactJS", "HTML/CSS", "Tailwind"}, Avatar = "/images/cv2.jpg", Status = "Đang tìm việc" },
            new() { Id = 3, Name = "Lê Hoàng C", Title = "Business Analyst", Experience = "5 năm", Skills = new[] {"BPMN", "Figma", "Agile/Scrum"}, Avatar = "/images/cv3.jpg", Status = "Đang tìm việc" },
            new() { Id = 4, Name = "Phạm D", Title = "DevOps Engineer", Experience = "4 năm", Skills = new[] {"Docker", "Kubernetes", "AWS"}, Avatar = "/images/cv4.jpg", Status = "Đang xem xét" },
            new() { Id = 5, Name = "Hoàng E", Title = "Mobile Developer", Experience = "1 năm", Skills = new[] {"Flutter", "Dart", "Firebase"}, Avatar = "/images/cv5.jpg", Status = "Đang tìm việc" }
        };
    }

    // LOGIC: Hàm lọc ứng viên dựa trên từ khóa và kỹ năng
    public static List<CandidateMockModel> FilterCandidates(string? keyword, string? skill)
    {
        var query = GetMockCandidates().AsQueryable();

        // Lọc theo từ khóa (Tên hoặc Chức danh)
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(keyword) || c.Title.ToLower().Contains(keyword));
        }

        // Lọc theo kỹ năng
        if (!string.IsNullOrWhiteSpace(skill))
        {
            skill = skill.ToLower();
            query = query.Where(c => c.Skills.Any(s => s.ToLower() == skill));
        }

        return query.ToList();
    }
}