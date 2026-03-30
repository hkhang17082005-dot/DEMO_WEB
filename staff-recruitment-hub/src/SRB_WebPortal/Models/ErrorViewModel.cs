namespace SRB_WebPortal.Models;

public record ErrorViewModel(string? RequestID, int StatusCode, string? Message)
{
   public string? RequestId { get; set; }

   public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

}
