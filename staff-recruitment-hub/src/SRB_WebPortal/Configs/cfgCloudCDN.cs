namespace SRB_WebPortal.Configs;

public class BunnyOptions
{
   public const string SectionName = "BunnySettings";

   public string StorageZoneName { get; set; } = string.Empty;
   public string Hostname { get; set; } = string.Empty;
   public string BaseCdnUrl { get; set; } = string.Empty;
   public string AccessKey { get; set; } = string.Empty;
}
