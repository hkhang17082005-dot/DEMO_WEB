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
         _logger.LogError(ex, "An Exception: {Message}", ex.Message);

         bool isApiRequest = context.Request.Path.StartsWithSegments("/api") || context.Request.Headers.XRequestedWith == "XMLHttpRequest";

         if (isApiRequest)
         {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = BaseResponse.Failure("Hệ thống có lỗi xảy ra, vui lòng thử lại sau!");

            await context.Response.WriteAsJsonAsync(response);
         }
         else
         {
            context.Response.StatusCode = 500;
            context.Request.Path = "/Home/Error";

            await _next(context);
         }
      }
   }
}
