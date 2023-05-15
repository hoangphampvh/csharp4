using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assiment_csad4.Migrations
{
    public partial class web_08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Product",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Product");
        }
    }
}
