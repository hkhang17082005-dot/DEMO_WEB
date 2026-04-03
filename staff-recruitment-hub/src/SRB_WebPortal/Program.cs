using Microsoft.AspNetCore.HttpOverrides;

using SRB_WebPortal.Shared;
using SRB_WebPortal.Extensions;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.apis.Job;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAppOptions(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExtensionServices();
// Trong Program.cs
builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
builder.Services.AddScoped<IJobPostService, JobPostService>();

builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(policy =>
   {
      policy.WithOrigins("http://localhost:8000", "http://localhost:8080")
             .AllowAnyHeader()
             .AllowAnyMethod();
   });
});

// Global Filter
builder.Services.AddControllers(options =>
{
   options.Filters.Add<AuthGuardAttribute>();
});

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

// Sử dụng HTTPS
// app.UseHttpsRedirection();

app.UseMiddleware<ExceptionFilter>();

app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
   ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthModule();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllers();

app.UseCors();

app.Run();
