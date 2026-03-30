using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRB_ViewModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_JobPosts_JobPostID",
                table: "JobApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplication_Users_UserID",
                table: "JobApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication");

            migrationBuilder.RenameTable(
                name: "JobApplication",
                newName: "JobApplications");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_UserID",
                table: "JobApplications",
                newName: "IX_JobApplications_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplication_JobPostID",
                table: "JobApplications",
                newName: "IX_JobApplications_JobPostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "ApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_JobPosts_JobPostID",
                table: "JobApplications",
                column: "JobPostID",
                principalTable: "JobPosts",
                principalColumn: "JobPostID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_UserID",
                table: "JobApplications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_JobPosts_JobPostID",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_UserID",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "JobApplication");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_UserID",
                table: "JobApplication",
                newName: "IX_JobApplication_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_JobPostID",
                table: "JobApplication",
                newName: "IX_JobApplication_JobPostID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplication",
                table: "JobApplication",
                column: "ApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_JobPosts_JobPostID",
                table: "JobApplication",
                column: "JobPostID",
                principalTable: "JobPosts",
                principalColumn: "JobPostID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplication_Users_UserID",
                table: "JobApplication",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
