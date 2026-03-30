namespace SRB_WebPortal.Configs;

public class JwtOptions
{
   public const string SectionName = "JwtSettings";

   public string SecretKey { get; set; } = "NQHxZeionDev";
   public string Issuer { get; set; } = string.Empty;
   public string Audience { get; set; } = string.Empty;
   public string EXP_ACCESS_TOKEN { get; set; } = "15m";
   public string EXP_REFRESH_TOKEN { get; set; } = "12h";
}
