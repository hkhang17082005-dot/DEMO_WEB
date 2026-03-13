using System.Text;

using System.Security.Cryptography;
using Microsoft.Extensions.Options;

using SRB_WebPortal.Configs;

namespace SRB_WebPortal.Services
{
   public interface IHashingService
   {
      /// <summary>
      /// Mã hóa chuỗi văn bản bằng thuật toán <b>BCrypt</b>
      /// </summary>
      /// <remarks>
      /// <b>Ví dụ sử dụng:</b>
      /// <br/>
      /// <c>string hash = HashValue("UserPassword");</c>
      /// </remarks>
      /// <returns>Chuỗi Hash hoàn chỉnh đã bao gồm Salt</returns>
      string HashValue(string value);
      /// <summary>
      /// Kiểm tra chính xác của chuỗi văn bản với mã băm <b>BCrypt</b>
      /// </summary>
      /// <remarks>
      /// <b>Ví dụ sử dụng:</b>
      /// <br/>
      /// <c>bool isValid = VerifyHashValue("UserPassword", "HashedPassword");</c>
      /// </remarks>
      /// <returns><c>True</c> nếu khớp ngược lại không khớp <c>False</c></returns>
      bool VerifyHashValue(string plaintText, string? hashValue);
      string ComputeHmacSha256(string message, string secretKey);
      string ComputeHmacSha512(string message, string secretKey);
   }

   public class HashingService(IOptions<ServiceOptions> serviceOptions) : IHashingService
   {
      private readonly string _hashSecret = serviceOptions.Value.HashSecret;

      public string HashValue(string value)
      {
         return BCrypt.Net.BCrypt.HashPassword(value + _hashSecret);
      }

      public bool VerifyHashValue(string plaintText, string? hashValue)
      {
         if (string.IsNullOrEmpty(hashValue)) return false;

         return BCrypt.Net.BCrypt.Verify(plaintText + _hashSecret, hashValue);
      }

      public string ComputeHmacSha256(string message, string secretKey)
      {
         var keyBytes = Encoding.UTF8.GetBytes(secretKey);
         var messageBytes = Encoding.UTF8.GetBytes(message);

         using var hmac = new HMACSHA256(keyBytes);
         var hashBytes = hmac.ComputeHash(messageBytes);

         return Convert.ToHexStringLower(hashBytes);
      }

      public string ComputeHmacSha512(string message, string secretKey)
      {
         var keyBytes = Encoding.UTF8.GetBytes(secretKey);
         var messageBytes = Encoding.UTF8.GetBytes(message);

         using var hmac = new HMACSHA512(keyBytes);
         var hashBytes = hmac.ComputeHash(messageBytes);

         return Convert.ToHexStringLower(hashBytes);
      }
   }
}
