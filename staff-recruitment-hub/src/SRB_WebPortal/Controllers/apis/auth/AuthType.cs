namespace SRB_WebPortal.Controllers.apis.auth
{
   public interface IUserPayload
   {
      string UserID { get; set; }
      string RoleSlug { get; set; }
      string Status { get; set; }
   }

   public class UserPayload : IUserPayload
   {
      public required string UserID { get; set; }
      public required string RoleSlug { get; set; }
      public required string Status { get; set; }
   }

   public class TokenPayload
   {
      public required UserPayload User { get; set; }
      public required string SessionID { get; set; }
      public required string RefreshToken { get; set; }
      public DateTime CreatedAt { get; set; }
   }

   public class DeviceInfo
   {
      public required string OS { get; set; }
      public required string Browser { get; set; }
      public required string Version { get; set; }
      public required string Device { get; set; }
      public required string IPAdress { get; set; }
   }

   public class SessionInfo
   {
      public required TokenPayload Payload { get; set; }
      public required DeviceInfo DeviceInfo { get; set; }
      public DateTime? LastAccessed { get; set; }
   }

   public class AuthResponse
   {
      public required UserPayload User { get; set; }
      public DateTime CreatedAt { get; set; }
   }

   public class SessionCacheInfo
   {
      public required string UserID { get; set; }
      public required string RefreshTokenHash { get; set; }
   }
}
