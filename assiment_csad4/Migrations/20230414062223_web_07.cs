using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assiment_csad4.Migrations
{
    public partial class web_07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_Name",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Role_Name",
                table: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Name",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_Name",
                table: "User",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Role_Name",
                table: "Role",
                column: "Name");
        }
    }
}
