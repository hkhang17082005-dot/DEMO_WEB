using SRB_WebPortal.Services;

namespace SRB_WebPortal.Shared;

public class TokenFactory(IHashingService hashing)
{
   private readonly IHashingService _hashing = hashing;

   public (string raw, string hash) CreateRefreshToken()
   {
      var raw = Guid.CreateVersion7().ToString();
      return (raw, _hashing.HashValue(raw));
   }

   public string CreateSessionId() => Guid.CreateVersion7().ToString();
}
