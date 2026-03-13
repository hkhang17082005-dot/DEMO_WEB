namespace SRB_WebPortal.Extensions
{
   public static class HttpContextExtensions
   {
      public static T? GetItem<T>(this HttpContext context, string key)
      {
         return context.Items.TryGetValue(key, out var value) && value is T result ? result : default;
      }
   }
}
