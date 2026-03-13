using Microsoft.AspNetCore.Mvc.Filters;

public class JwtGuard : ActionFilterAttribute
{
   public override void OnActionExecuting(ActionExecutingContext context)
   {
      Console.WriteLine("Start");
   }
}
