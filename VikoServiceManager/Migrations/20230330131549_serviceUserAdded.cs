using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikoServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class serviceUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Service",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_ApplicationUserId",
                table: "Service",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_AspNetUsers_ApplicationUserId",
                table: "Service",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_AspNetUsers_ApplicationUserId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_ApplicationUserId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Service");
        }
    }
}
