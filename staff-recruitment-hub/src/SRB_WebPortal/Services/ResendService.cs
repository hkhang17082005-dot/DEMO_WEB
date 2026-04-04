using Resend;
using System.Threading.Channels;

namespace SRB_WebPortal.Services;

public interface IResendService
{
   ValueTask SendMailAsync(string to, string subject, string body);

   ChannelReader<EmailJob> Reader { get; }
}

public class ResendService : IResendService
{
   private readonly Channel<EmailJob> _queue;

   public ResendService()
   {
      _queue = Channel.CreateUnbounded<EmailJob>();
   }

   public async ValueTask SendMailAsync(string to, string subject, string body)
   {
      var job = new EmailJob { To = to, Subject = subject, Body = body };

      await _queue.Writer.WriteAsync(job);
   }

   // Hàm Worker Send Mail
   public ChannelReader<EmailJob> Reader => _queue.Reader;
}

public class EmailBackgroundWorker(
   IResendService resendService,
   IServiceProvider serviceProvider,
   ILogger<EmailBackgroundWorker> logger
) : BackgroundService
{
   private readonly IResendService _resendService = resendService;
   private readonly IServiceProvider _serviceProvider = serviceProvider;
   private readonly ILogger<EmailBackgroundWorker> _logger = logger;

   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
   {
      _logger.LogInformation("Email Background Worker đang chạy...");

      await foreach (var job in _resendService.Reader.ReadAllAsync(stoppingToken))
      {
         try
         {
            using var scope = _serviceProvider.CreateScope();
            var resendClient = scope.ServiceProvider.GetRequiredService<IResend>();

            var message = new EmailMessage
            {
               From = "onboarding@zeion.online",
               To = job.To,
               Subject = job.Subject,
               HtmlBody = job.Body
            };

            await resendClient.EmailSendAsync(message, stoppingToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
               _logger.LogInformation("Đã gửi mail thành công tới: {EmailRecipient}", job.To);
            }
         }
         catch (Exception ex)
         {
            if (_logger.IsEnabled(LogLevel.Error))
            {
               _logger.LogError(ex, "Lỗi khi gửi mail tới {EmailRecipient}", job.To);
            }
         }
      }
   }
}

public class EmailJob
{
   public string To { get; set; } = string.Empty;

   public string Subject { get; set; } = string.Empty;

   public string Body { get; set; } = string.Empty;

}
