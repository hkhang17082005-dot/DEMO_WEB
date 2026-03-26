namespace SRB_WebPortal.Controllers;

[AttributeUsage(AttributeTargets.All)]
public class SubdomainAttribute(string subdomain) : Attribute, IRouteConstraint
{
   private readonly string _subdomain = subdomain;

   public bool Match(
      HttpContext? httpContext,
      IRouter? route,
      string parameterName,
      RouteValueDictionary values,
      RouteDirection routeDirection
   )
   {
      if (httpContext == null) return false;
      var host = httpContext.Request.Host.Host;

      return host.StartsWith($"{_subdomain}.");
   }
}

