using Microsoft.EntityFrameworkCore;
using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Data
{
    public static class BusinessSeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DatabaseContext>();
            var businessSet = context.Set<BusinessProfile>();
            var userSet = context.Set<User>();
            var roleSet = context.Set<Role>();
            var typeRoleSet = context.Set<TypeRole>();

            if (await businessSet.AnyAsync()) return;

            // 1. SỬA LỖI CS9035: Gán đúng TypeName và TypeSlug cho TypeRole
            var typeRole = new TypeRole
            {
                TypeName = "Management",
                TypeSlug = "management"
            };
            typeRoleSet.Add(typeRole);
            await context.SaveChangesAsync();

            // 2. Tạo Role "Business Manager"
            var businessRole = new Role
            {
                RoleName = "Business Manager",
                RoleSlug = "business-manager",
                TypeRoleID = typeRole.TypeRoleID // Gán khóa ngoại từ TypeRole vừa tạo
            };
            roleSet.Add(businessRole);
            await context.SaveChangesAsync();

            // 3. Tạo Doanh nghiệp mẫu
            var testBusiness = new BusinessProfile
            {
                CompanyName = "SRB Tech Solutions",
                Website = "https://srb-tech.com",
                Description = "Giải pháp nhân sự thông minh"
            };
            businessSet.Add(testBusiness);
            await context.SaveChangesAsync();

            // 4. Tạo User mẫu
            var testUser = new User
            {
                UserID = "dev-test-01",
                Username = "business_admin",
                Role = businessRole, // Gán đối tượng Role vừa tạo
                BusinessID = testBusiness.Id // Gán ID kiểu int của doanh nghiệp
            };

            // Lưu ý: Email và Password vẫn bị loại bỏ vì lớp User của bạn không có

            userSet.Add(testUser);
            await context.SaveChangesAsync();
        }
    }
}