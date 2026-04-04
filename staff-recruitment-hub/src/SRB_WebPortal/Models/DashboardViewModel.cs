namespace SRB_WebPortal.Models;

public class DashboardViewModel
{
    public int TotalCandidates { get; set; }
    public int TotalJobPosts { get; set; }
    public int TotalApplications { get; set; }
    public int TotalBusinesses { get; set; }
    public double CandidatesGrowth { get; set; } // % tăng so với tháng trước
    public double JobPostsGrowth { get; set; }
    public double ApplicationsGrowth { get; set; }
    public double BusinessesGrowth { get; set; }
    // Có thể thêm data cho charts và recent applications sau
}