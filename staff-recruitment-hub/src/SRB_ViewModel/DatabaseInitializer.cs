using Microsoft.EntityFrameworkCore;

using SRB_ViewModel.Data;
using SRB_ViewModel.Entities;

namespace SRB_ViewModel;

public static class DatabaseInitializer
{
   public static void InitCoreData(this ModelBuilder modelBuilder)
   {
      // Seed TypeRole
      modelBuilder.Entity<TypeRole>().HasData(
         new TypeRole { TypeRoleID = 1, TypeName = "System", TypeSlug = RoleTypes.SYSTEM },
         new TypeRole { TypeRoleID = 2, TypeName = "Business", TypeSlug = RoleTypes.BUSINESS },
         new TypeRole { TypeRoleID = 3, TypeName = "Candidate", TypeSlug = RoleTypes.CANDIDATE }
      );

      // Seed Role
      modelBuilder.Entity<Role>().HasData(
         /* System RoleID [1 - 20] */
         new Role { RoleID = 1, RoleName = "Admin", RoleSlug = Roles.ADMIN, TypeRoleID = 1, RoleDesc = "Toàn quyền hệ thống" },
         new Role { RoleID = 2, RoleName = "System Manager", RoleSlug = Roles.SYSTEM_MANAGER, TypeRoleID = 1, RoleDesc = "Vận hành hệ thống" },
         new Role { RoleID = 3, RoleName = "Support", RoleSlug = Roles.SUPPORT, TypeRoleID = 1, RoleDesc = "Hỗ trợ người dùng" },

         /* Business [20 - 50] */
         new Role { RoleID = 20, RoleName = "Business Manager", RoleSlug = Roles.BUSINESS_MANAGER, TypeRoleID = 2, RoleDesc = "Quản lý business" },
         new Role { RoleID = 21, RoleName = "Hiring Manager", RoleSlug = Roles.HIRING_MANAGER, TypeRoleID = 2, RoleDesc = "Quản lý tuyển dụng cấp cao" },
         new Role { RoleID = 22, RoleName = "HR Manager", RoleSlug = Roles.HR_MANAGER, TypeRoleID = 2, RoleDesc = "Quản lý nhân sự" },
         new Role { RoleID = 23, RoleName = "Recruiter", RoleSlug = Roles.RECRUITER, TypeRoleID = 2, RoleDesc = "Tuyển dụng" },
         new Role { RoleID = 24, RoleName = "Interviewer", RoleSlug = Roles.INTERVIEWER, TypeRoleID = 2, RoleDesc = "Phỏng vấn" },
         new Role { RoleID = 25, RoleName = "Collaborator", RoleSlug = Roles.COLLABORATOR, TypeRoleID = 2, RoleDesc = "Cộng tác viên tạm thời" },

         /* Other [50] */
         new Role { RoleID = 50, RoleName = "Candidate", RoleSlug = Roles.CANDIDATE, TypeRoleID = 3, RoleDesc = "Ứng viên" }
      );

      // Seed Permission
      modelBuilder.Entity<Permission>().HasData(
         /* System PerID [1 - 20] */
         new Permission { PerID = 1, PerName = "Read Users", PerSlug = Permissions.READ_USERS, PerDesc = "Xem danh sách người dùng trong hệ thống" },
         new Permission { PerID = 2, PerName = "Write Users", PerSlug = Permissions.WRITE_USER, PerDesc = "Cập nhật thông tin người dùng" },
         new Permission { PerID = 3, PerName = "Assign Roles", PerSlug = Permissions.ASSIGN_ROLE, PerDesc = "Gán vai trò và quyền hạn cho tài khoản" },
         new Permission { PerID = 4, PerName = "Ban Users", PerSlug = Permissions.BAN_USER, PerDesc = "Tạm dừng hoặc khóa tài khoản người dùng vi phạm" },
         new Permission { PerID = 5, PerName = "View System Logs", PerSlug = Permissions.VIEW_SYSTEM_LOGS, PerDesc = "Theo dõi lịch sử hoạt động của hệ thống" },

         /* Manager / Support ID [20 - 40] */
         new Permission { PerID = 20, PerName = "Lock Jobs", PerSlug = Permissions.LOCK_JOB, PerDesc = "Tạm khóa bài tuyển dụng vi phạm quy định" },
         new Permission { PerID = 21, PerName = "Hide Jobs", PerSlug = Permissions.HIDE_JOB, PerDesc = "Ẩn bài tuyển dụng khỏi kết quả tìm kiếm" },
         new Permission { PerID = 22, PerName = "Delete Jobs", PerSlug = Permissions.DELETE_JOB, PerDesc = "Xóa vĩnh viễn bài tuyển dụng" },
         new Permission { PerID = 23, PerName = "Moderate Reports", PerSlug = Permissions.MODERATE_REPORT, PerDesc = "Phê duyệt hoặc từ chối các báo cáo vi phạm" },
         new Permission { PerID = 24, PerName = "Restrict Business", PerSlug = Permissions.RESTRICT_BUSINESS, PerDesc = "Hạn chế quyền của doanh nghiệp vi phạm" },

         /* Business ID [40 - 60] */
         new Permission { PerID = 40, PerName = "Manage Business", PerSlug = Permissions.MANAGE_BUSINESS, PerDesc = "Cập nhật thông tin và hồ sơ doanh nghiệp" },
         new Permission { PerID = 41, PerName = "People Management", PerSlug = Permissions.PEOPLE_BUSINESS, PerDesc = "Quản lý nhân sự cho doanh nghiệp" },
         new Permission { PerID = 42, PerName = "Create Jobs", PerSlug = Permissions.CREATE_JOB, PerDesc = "Đăng tin tuyển dụng mới" },
         new Permission { PerID = 43, PerName = "Manage Jobs", PerSlug = Permissions.MANAGER_JOB, PerDesc = "Có quyền thay đổi tin tuyển dụng" },
         new Permission { PerID = 44, PerName = "Approve Jobs", PerSlug = Permissions.APPROVE_JOB, PerDesc = "Phê duyệt tin tuyển dụng để hiển thị công khai" },
         new Permission { PerID = 45, PerName = "Manage Templates", PerSlug = Permissions.MANAGE_TEMPLATE, PerDesc = "Quản lý các mẫu email, mẫu tin tuyển dụng" },
         new Permission { PerID = 46, PerName = "View Candidates", PerSlug = Permissions.VIEW_CANDIDATE, PerDesc = "Xem chi tiết CV và hồ sơ ứng viên" },
         new Permission { PerID = 47, PerName = "Interview Candidates", PerSlug = Permissions.INTERVIEW_CANDIDATE, PerDesc = "Lên lịch và gửi lời mời phỏng vấn" },

         /* Candidate ID [60] */
         new Permission { PerID = 60, PerName = "Apply Jobs", PerSlug = Permissions.APPLY_JOB, PerDesc = "Gửi hồ sơ ứng tuyển vào vị trí mong muốn" }
      );

      // Seed Role Permission
      modelBuilder.Entity<RolePermission>().HasData(
         // Admin
         new RolePermission { RoleID = 1, PerID = 1 },  // READ_USER
         new RolePermission { RoleID = 1, PerID = 2 },  // WRITE_USER
         new RolePermission { RoleID = 1, PerID = 3 },  // ASSIGN_ROLE
         new RolePermission { RoleID = 1, PerID = 4 },  // BAN_USER
         new RolePermission { RoleID = 1, PerID = 5 },  // VIEW_SYSTEM_LOGS
         new RolePermission { RoleID = 1, PerID = 20 }, // LOCK_JOB
         new RolePermission { RoleID = 1, PerID = 21 }, // HIDE_JOB
         new RolePermission { RoleID = 1, PerID = 22 }, // DELETE_JOB
         new RolePermission { RoleID = 1, PerID = 23 }, // MODERATE_REPORT
         new RolePermission { RoleID = 1, PerID = 24 }, // RESTRICT_BUSINESS

         // Manager
         new RolePermission { RoleID = 2, PerID = 1 },  // READ_USER
         new RolePermission { RoleID = 2, PerID = 4 },  // BAN_USER
         new RolePermission { RoleID = 2, PerID = 5 },  // VIEW_SYSTEM_LOGS
         new RolePermission { RoleID = 2, PerID = 20 }, // LOCK_JOB
         new RolePermission { RoleID = 2, PerID = 21 }, // HIDE_JOB
         new RolePermission { RoleID = 2, PerID = 23 }, // MODERATE_REPORT
         new RolePermission { RoleID = 2, PerID = 24 }, // RESTRICT_BUSINESS

         // Support
         new RolePermission { RoleID = 3, PerID = 1 },  // READ_USER
         new RolePermission { RoleID = 3, PerID = 20 }, // LOCK_JOB
         new RolePermission { RoleID = 3, PerID = 21 }, // HIDE_JOB
         new RolePermission { RoleID = 3, PerID = 23 }, // MODERATE_REPORT

         // Business Manager
         new RolePermission { RoleID = 20, PerID = 40 }, // MANAGE_BUSINESS
         new RolePermission { RoleID = 20, PerID = 41 }, // PEOPLE_BUSINESS
         new RolePermission { RoleID = 20, PerID = 42 }, // CREATE_JOB
         new RolePermission { RoleID = 20, PerID = 43 }, // MANAGER_JOB
         new RolePermission { RoleID = 20, PerID = 44 }, // APPROVE_JOB
         new RolePermission { RoleID = 20, PerID = 45 }, // MANAGE_TEMPLATE
         new RolePermission { RoleID = 20, PerID = 46 }, // VIEW_CANDIDATE
         new RolePermission { RoleID = 20, PerID = 47 }, // INTERVIEW_CANDIDATE

         // Hiring Manager
         new RolePermission { RoleID = 21, PerID = 42 }, // CREATE_JOB
         new RolePermission { RoleID = 21, PerID = 43 }, // MANAGER_JOB
         new RolePermission { RoleID = 21, PerID = 44 }, // APPROVE_JOB
         new RolePermission { RoleID = 21, PerID = 46 }, // VIEW_CANDIDATE
         new RolePermission { RoleID = 21, PerID = 47 }, // INTERVIEW_CANDIDATE

         // HR Manager
         new RolePermission { RoleID = 22, PerID = 41 }, // PEOPLE_BUSINESS
         new RolePermission { RoleID = 22, PerID = 46 }, // VIEW_CANDIDATE
         new RolePermission { RoleID = 22, PerID = 47 }, // INTERVIEW_CANDIDATE
         new RolePermission { RoleID = 22, PerID = 45 }, // MANAGE_TEMPLATE

         // Recruiter
         new RolePermission { RoleID = 23, PerID = 42 }, // CREATE_JOB
         new RolePermission { RoleID = 23, PerID = 43 }, // MANAGER_JOB
         new RolePermission { RoleID = 23, PerID = 46 }, // VIEW_CANDIDATE

         // Interviewer
         new RolePermission { RoleID = 24, PerID = 46 }, // VIEW_CANDIDATE
         new RolePermission { RoleID = 24, PerID = 47 }, // INTERVIEW_CANDIDATE

         // Collaborator
         // new RolePermission { RoleID = 25, PerID = 42 }, // CREATE_JOB
         // new RolePermission { RoleID = 25, PerID = 46 }, // VIEW_CANDIDATE

         // Candidate
         new RolePermission { RoleID = 50, PerID = 60 }  // APPLY_JOB
      );
   }
}
