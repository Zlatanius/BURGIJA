using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Migrations
{
    public partial class SeventhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Tool");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tool");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ToolType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ToolType",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ToolType");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ToolType");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Tool",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
