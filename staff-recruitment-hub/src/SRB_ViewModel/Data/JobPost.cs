namespace SRB_ViewModel.Data;

public static class JobTypeHelper
{
   private static readonly Dictionary<int, string> JobTypeNames = new()
   {
      { 1, "Toàn thời gian" },
      { 2, "Bán thời gian" },
      { 3, "Hợp đồng" },
      { 4, "Thực tập" },
      { 5, "Làm việc từ xa" },
      { 6, "Hybrid" },
      { 7, "Freelance" },
      { 8, "Thời vụ" }
    };

   public static string GetName(int id)
   {
      return JobTypeNames.TryGetValue(id, out string? value) ? value : "Khác";
   }

   public static Dictionary<int, string> GetAll() => JobTypeNames;
}
