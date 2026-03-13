using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRB_ViewModel.Migrations
{
    /// <inheritdoc />
    public partial class ReconcileMergeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            // migrationBuilder.AddColumn<string>(
            //     name: "CompanyName",
            //     table: "Jobs",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            // migrationBuilder.DropColumn(
            //     name: "CompanyName",
            //     table: "Jobs");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }
    }
}
