using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using SRB_WebPortal.Shared;
using SRB_WebPortal.Configs;
using SRB_WebPortal.Controllers.apis.auth;

namespace SRB_WebPortal.Services
{
   public interface IJwtService
   {
      /// <summary>
      /// Tạo JSON Web Token (JWT) dựa trên thông tin người dùng và phiên làm việc.
      /// </summary>
      /// <remarks>
      /// <b>Ví dụ sử dụng:</b>
      /// <br/>
      /// <c>string token = GenerateToken(tokenPayload);</c>
      /// </remarks>
      /// <returns>Chuỗi JWT đã được ký và mã hóa</returns>
      string GenerateToken(TokenPayload tokenPayload, DateTime? expires = null);

      /// <summary>
      /// Xác thực và kiểm tra tính hợp lệ của JSON Web Token (JWT).
      /// </summary>
      /// <remarks>
      /// Phương thức sẽ kiểm tra chữ ký, thời hạn (expiration),
      /// issuer và audience của token.
      /// </remarks>
      /// <returns>
      /// <c>true</c> nếu token hợp lệ; ngược lại <c>false</c>
      /// </returns>
      ClaimsPrincipal? ValidateToken(string token, out string failureReason);
   }

   public class JwtService : IJwtService
   {
      private readonly IOptions<JwtOptions> _jwtOptions;

      public JwtService(IOptions<JwtOptions> jwtOptions)
      {
         _jwtOptions = jwtOptions;
      }

      public string GenerateToken(TokenPayload tokenPayload, DateTime? expires = null)
      {
         var claims = new List<Claim>
      {
         // Thông tin User
         new Claim("user_id", tokenPayload.User.UserID),
         new Claim("user_role", tokenPayload.User.RoleSlug),
         new Claim("user_status", tokenPayload.User.Status),

         // Thông tin Token/Session
         new Claim(JwtRegisteredClaimNames.Jti, tokenPayload.SessionID),
         new Claim("refresh_token", tokenPayload.RefreshToken),

         new Claim("iat", new DateTimeOffset(tokenPayload.CreatedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
      };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         var newToken = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: expires ?? DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
         );

         return new JwtSecurityTokenHandler().WriteToken(newToken);
      }

      public ClaimsPrincipal? ValidateToken(string token, out string failureReason)
      {
         var tokenHandler = new JwtSecurityTokenHandler();
         var secretKey = Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey);
         failureReason = "Unknown";

         try
         {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(secretKey),
               ValidateIssuer = true,
               ValidIssuer = _jwtOptions.Value.Issuer,
               ValidateAudience = true,
               ValidAudience = _jwtOptions.Value.Audience,
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
         }
         catch (SecurityTokenExpiredException)
         {
            failureReason = BackendSignals.SESSION_EXPIRED;
            return null;
         }
         catch (SecurityTokenInvalidSignatureException)
         {
            failureReason = BackendSignals.TAMPERED_TOKEN;
            return null;
         }
         catch
         {
            failureReason = BackendSignals.INVALID_TOKEN;
            return null;
         }
      }
   }
}
