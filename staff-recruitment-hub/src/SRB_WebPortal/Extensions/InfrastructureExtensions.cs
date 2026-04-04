using System.Text;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using SRB_ViewModel;
using Resend;

namespace SRB_WebPortal.Extensions
{
   public static class InfrastructureExtensions
   {
      public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
      {
         // Setup Database
         var connectionDatabaseString = configuration.GetConnectionString("ConnectionDatabase");
         services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionDatabaseString));

         // Setup Redis
         string connectionRedisString = configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
         services.AddStackExchangeRedisCache(options =>
         {
            options.Configuration = connectionRedisString;
            options.InstanceName = "SampleInstance:";
         });

         var resendSection = configuration.GetSection("ResendSettings:APIKey");
         services.AddHttpClient<IResend, ResendClient>();
         services.Configure<ResendClientOptions>(options =>
         {
            options.ApiToken = configuration["ResendSettings:ApiToken"] ?? string.Empty;
         });


         // Setup JWT Authentication
         services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(options =>
         {
            options.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = configuration["JwtSettings:Issuer"],
               ValidAudience = configuration["JwtSettings:Audience"],
               ClockSkew = TimeSpan.Zero,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? "NQHxZeionDev"))
            };
         });

         // Setup Cloudinary
         var cloudinarySettings = configuration.GetSection("CloudinarySettings");
         Account account = new(
            cloudinarySettings["CloudName"],
            cloudinarySettings["APIKey"],
            cloudinarySettings["APISecret"]
         );
         Cloudinary cloudinary = new(account);
         cloudinary.Api.Secure = true;
         services.AddSingleton(cloudinary);

         return services;
      }
   }
}
