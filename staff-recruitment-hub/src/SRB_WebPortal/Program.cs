using Microsoft.AspNetCore.HttpOverrides;

using SRB_WebPortal.Shared;
using SRB_WebPortal.Extensions;
using SRB_WebPortal.Controllers.apis.auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAppOptions(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExtensionServices();

// Global Filter
builder.Services.AddControllers(options =>
{
   options.Filters.Add<AuthGuardAttribute>();
});

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Home/Error");
   app.UseHsts();
}

// Sử dụng HTTPS
// app.UseHttpsRedirection();

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

app.UseMiddleware<ExceptionFilter>();

app.MapControllers();

app.Run();
