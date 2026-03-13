using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SRB_ViewModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    PerSlug = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PerDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PerID);
                });

            migrationBuilder.CreateTable(
                name: "TypeRole",
                columns: table => new
                {
                    TypeRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    TypeSlug = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TypeDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeRole", x => x.TypeRoleID);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobID);
                    table.ForeignKey(
                        name: "FK_Jobs_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    RoleSlug = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    TypeRoleID = table.Column<int>(type: "int", nullable: false),
                    RoleDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                    table.ForeignKey(
                        name: "FK_Roles_TypeRole_TypeRoleID",
                        column: x => x.TypeRoleID,
                        principalTable: "TypeRole",
                        principalColumn: "TypeRoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PerID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleID, x.PerID });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PerID",
                        column: x => x.PerID,
                        principalTable: "Permission",
                        principalColumn: "PerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermission_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    HashPassword = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "char(36)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PerID", "PerDesc", "PerName", "PerSlug" },
                values: new object[,]
                {
                    { 1, "Xem danh sách người dùng trong hệ thống", "Read Users", "READ_USER" },
                    { 2, "Cập nhật thông tin người dùng", "Write Users", "WRITE_USER" },
                    { 3, "Gán vai trò và quyền hạn cho tài khoản", "Assign Roles", "ASSIGN_ROLE" },
                    { 4, "Tạm dừng hoặc khóa tài khoản người dùng vi phạm", "Ban Users", "BAN_USER" },
                    { 5, "Theo dõi lịch sử hoạt động của hệ thống", "View System Logs", "VIEW_SYSTEM_LOGS" },
                    { 20, "Tạm khóa bài tuyển dụng vi phạm quy định", "Lock Jobs", "LOCK_JOB" },
                    { 21, "Ẩn bài tuyển dụng khỏi kết quả tìm kiếm", "Hide Jobs", "HIDE_JOB" },
                    { 22, "Xóa vĩnh viễn bài tuyển dụng", "Delete Jobs", "DELETE_JOB" },
                    { 23, "Phê duyệt hoặc từ chối các báo cáo vi phạm", "Moderate Reports", "MODERATE_REPORT" },
                    { 24, "Hạn chế quyền của doanh nghiệp vi phạm", "Restrict Business", "RESTRICT_BUSINESS" },
                    { 40, "Cập nhật thông tin và hồ sơ doanh nghiệp", "Manage Business", "MANAGE_BUSINESS" },
                    { 41, "Quản lý nhân sự cho doanh nghiệp", "People Management", "PEOPLE_BUSINESS" },
                    { 42, "Đăng tin tuyển dụng mới", "Create Jobs", "CREATE_JOB" },
                    { 43, "Có quyền thay đổi tin tuyển dụng", "Manage Jobs", "MANAGER_JOB" },
                    { 44, "Phê duyệt tin tuyển dụng để hiển thị công khai", "Approve Jobs", "APPROVE_JOB" },
                    { 45, "Quản lý các mẫu email, mẫu tin tuyển dụng", "Manage Templates", "MANAGE_TEMPLATE" },
                    { 46, "Xem chi tiết CV và hồ sơ ứng viên", "View Candidates", "VIEW_CANDIDATE" },
                    { 47, "Lên lịch và gửi lời mời phỏng vấn", "Interview Candidates", "INTERVIEW_CANDIDATE" },
                    { 60, "Gửi hồ sơ ứng tuyển vào vị trí mong muốn", "Apply Jobs", "APPLY_JOB" }
                });

            migrationBuilder.InsertData(
                table: "TypeRole",
                columns: new[] { "TypeRoleID", "TypeDesc", "TypeName", "TypeSlug" },
                values: new object[,]
                {
                    { 1, null, "System", "SYSTEM" },
                    { 2, null, "Business", "BUSINESS" },
                    { 3, null, "Candidate", "CANDIDATE" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "RoleDesc", "RoleName", "RoleSlug", "TypeRoleID" },
                values: new object[,]
                {
                    { 1, "Toàn quyền hệ thống", "Admin", "ADMIN", 1 },
                    { 2, "Vận hành hệ thống", "System Manager", "SYSTEM_MANAGER", 1 },
                    { 3, "Hỗ trợ người dùng", "Support", "SUPPORT", 1 },
                    { 20, "Quản lý business", "Business Manager", "BUSINESS_MANAGER", 2 },
                    { 21, "Quản lý tuyển dụng cấp cao", "Hiring Manager", "HIRING_MANAGER", 2 },
                    { 22, "Quản lý nhân sự", "HR Manager", "HR_MANAGER", 2 },
                    { 23, "Tuyển dụng", "Recruiter", "RECRUITER", 2 },
                    { 24, "Phỏng vấn", "Interviewer", "INTERVIEWER", 2 },
                    { 25, "Cộng tác viên tạm thời", "Collaborator", "COLLABORATOR", 2 },
                    { 50, "Ứng viên", "Candidate", "CANDIDATE", 3 }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PerID", "RoleID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 1, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 20, 2 },
                    { 21, 2 },
                    { 23, 2 },
                    { 24, 2 },
                    { 1, 3 },
                    { 20, 3 },
                    { 21, 3 },
                    { 23, 3 },
                    { 40, 20 },
                    { 41, 20 },
                    { 42, 20 },
                    { 43, 20 },
                    { 44, 20 },
                    { 45, 20 },
                    { 46, 20 },
                    { 47, 20 },
                    { 42, 21 },
                    { 43, 21 },
                    { 44, 21 },
                    { 46, 21 },
                    { 47, 21 },
                    { 41, 22 },
                    { 45, 22 },
                    { 46, 22 },
                    { 47, 22 },
                    { 42, 23 },
                    { 43, 23 },
                    { 46, 23 },
                    { 46, 24 },
                    { 47, 24 },
                    { 60, 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_LocationID",
                table: "Jobs",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PerName",
                table: "Permission",
                column: "PerName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PerSlug",
                table: "Permission",
                column: "PerSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PerID",
                table: "RolePermission",
                column: "PerID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleSlug",
                table: "Roles",
                column: "RoleSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TypeRoleID",
                table: "Roles",
                column: "TypeRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_TypeRole_TypeName",
                table: "TypeRole",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeRole_TypeSlug",
                table: "TypeRole",
                column: "TypeSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleID",
                table: "UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TypeRole");
        }
    }
}
