using SRB_WebPortal.Services;

public class TokenFactory
{
   private readonly IHashingService _hashing;

   public TokenFactory(IHashingService hashing)
   {
      _hashing = hashing;
   }

   public (string raw, string hash) CreateRefreshToken()
   {
      var raw = Guid.CreateVersion7().ToString();
      return (raw, _hashing.HashValue(raw));
   }

   public string CreateSessionId() => Guid.CreateVersion7().ToString();
}
