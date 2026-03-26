namespace SRB_WebPortal.Shared;

class ExceptionFilter(RequestDelegate next, ILogger<ExceptionFilter> logger)
{
   private readonly RequestDelegate _next = next;
   private readonly ILogger<ExceptionFilter> _logger = logger;

   public async Task InvokeAsync(HttpContext context)
   {
      try
      {
         await _next(context);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

         context.Response.ContentType = "application/json";
         context.Response.StatusCode = 500;

         var response = BaseResponse.Failure("Hệ thống có lỗi xảy ra, vui lòng thử lại sau!");

         await context.Response.WriteAsJsonAsync(response);
      }
   }
}
