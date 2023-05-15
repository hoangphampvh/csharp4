using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assiment_csad4.Migrations
{
    public partial class web_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CartDetail",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CartDetail");
        }
    }
}
