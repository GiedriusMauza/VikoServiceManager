using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikoServiceManager.Migrations
{
    /// <inheritdoc />
    public partial class ResidentGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResidentGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResidentGroupMembership",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResidentGroupId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentGroupMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResidentGroupMembership_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResidentGroupMembership_ResidentGroup_ResidentGroupId",
                        column: x => x.ResidentGroupId,
                        principalTable: "ResidentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResidentGroupMembership_ApplicationUserId",
                table: "ResidentGroupMembership",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidentGroupMembership_ResidentGroupId",
                table: "ResidentGroupMembership",
                column: "ResidentGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResidentGroupMembership");

            migrationBuilder.DropTable(
                name: "ResidentGroup");
        }
    }
}
