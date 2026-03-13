namespace SRB_WebPortal.Configs
{
   public class SystemOptions
   {
      public const string SectionName = "SystemSettings";

      public string HostServer { get; set; } = "localhost";
      public int PortServer { get; set; } = 5000;
      public string Environment { get; set; } = "Development";

      public string ServerDomain => $"{(Environment == "Development" ? "http" : "https")}://{HostServer}:{PortServer}/";
   }
}
