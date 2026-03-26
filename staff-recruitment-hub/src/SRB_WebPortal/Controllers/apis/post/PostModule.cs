namespace SRB_WebPortal.Controllers.apis.post;

public static class PostModule
{
   public static IServiceCollection AddPostModule(this IServiceCollection services)
   {
      services.AddScoped<IPostRepository, PostRepository>();

      services.AddScoped<IPostService, PostService>();

      return services;
   }
}
