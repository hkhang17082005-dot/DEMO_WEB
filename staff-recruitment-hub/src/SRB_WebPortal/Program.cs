using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using SRB_ViewModel;
using SRB_WebPortal.Controllers;
using SRB_WebPortal.Controllers.apis.auth;
using SRB_WebPortal.Controllers.Apis.Business;
using SRB_WebPortal.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<BusinessRepository>();
builder.Services.AddScoped<BusinessService>();

// Add services to the container
builder.Services.AddAppOptions(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddBusinessServices();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Đăng ký Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
   // Cấu hình mật khẩu
       options.Password.RequireDigit = false; // Không bắt buộc có số
       options.Password.RequireLowercase = false; // Không bắt buộc chữ thường
       options.Password.RequireNonAlphanumeric = false; // Không bắt buộc ký tự đặc biệt (!@#)
       options.Password.RequireUppercase = false; // Không bắt buộc chữ hoa
       options.Password.RequiredLength = 6; // Độ dài tối thiểu
}) 
   .AddRoles<IdentityRole>() // Kích hoạt tính năng Phân quyền
   .AddEntityFrameworkStores<DatabaseContext>();

//  Ghi đè đường dẫn Login mặc định của Identity
builder.Services.ConfigureApplicationCookie(options =>
{
   options.LoginPath = "/Login/Index"; // Đường dẫn tới trang Login của bạn
   options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
   options.AccessDeniedPath = "/Account/AccessDenied"; // Trang báo lỗi khi không có quyền
   options.Events.OnRedirectToAccessDenied = context =>
   {
       context.Response.Redirect(options.AccessDeniedPath); // Chuyển hướng thẳng, không kèm tham số
       return Task.CompletedTask;
   };
});


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

// Use Sub Domain
app.Use(async (context, next) =>
{
   var host = context.Request.Host.Host;

   if (host.StartsWith("manager."))
   {
      context.Items["IsAdminArea"] = true;
   }

   await next();
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
   ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthModule();

app.MapControllerRoute(
   name: "subdomain",
   pattern: "{controller=Dashboard}/{action=Index}/{id?}",
   constraints: new { host = new SubdomainAttribute("manager") }
);

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.UseMiddleware<ExceptionFilter>();

app.MapControllers();

app.Run();
