using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SRB_WebPortal.Services
{
   public interface IRedisService
   {
      /// <summary>
      /// Lưu trữ một đối tượng vào Redis cache với một khóa xác định.
      /// </summary>
      /// <typeparam name="T">Kiểu dữ liệu của đối tượng cần lưu trữ</typeparam>
      /// <returns>Một Task đại diện cho quá trình thực thi bất đồng bộ</returns>
      Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

      /// <summary>
      /// Lấy dữ liệu từ Redis cache dựa trên khóa
      /// </summary>
      /// <returns>
      /// Trả về đối tượng kiểu <typeparamref name="T"/> nếu tìm thấy;
      /// ngược lại trả về <c>null</c>
      /// </returns>
      Task<T?> GetAsync<T>(string key);

      /// <summary>
      /// Xóa dữ liệu khỏi Redis cache dựa trên khóa
      /// </summary>
      /// <returns>Một Task đại diện cho quá trình thực thi bất đồng bộ</returns>
      Task RemoveAsync(string key);
   }

   public static class RedisCacheKeys
   {
      public const string SystemPrefix = "system";
      public static string RoleKey(string roleName) => $"{SystemPrefix}:role:{roleName}";
      public static string SessionInfo(string userId, string sessionId) => $"user:{userId}:session:{sessionId}:info";
      public static string RefreshToken(string sessionId) => $"session:{sessionId}:refreshToken";
   }

   public class RedisService(IDistributedCache cache) : IRedisService
   {
      private readonly IDistributedCache _cache = cache;

      // Lưu dữ liệu vào Cache
      public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
      {
         var options = new DistributedCacheEntryOptions
         {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
         };
         var jsonData = JsonSerializer.Serialize(value);
         await _cache.SetStringAsync(key, jsonData, options);
      }

      // Lấy dữ liệu từ Cache
      public async Task<T?> GetAsync<T>(string key)
      {
         var jsonData = await _cache.GetStringAsync(key);
         if (string.IsNullOrEmpty(jsonData))
         {
            return default;
         }
         try
         {
            return JsonSerializer.Deserialize<T>(jsonData);
         }
         catch (JsonException)
         {
            return default;
         }
      }

      // Xóa Cache
      public async Task RemoveAsync(string key)
      {
         await _cache.RemoveAsync(key);
      }
   }
}
