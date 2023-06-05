using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Migrations
{
    public partial class SixthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tool");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ToolType",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ToolType");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
