using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Data.Migrations
{
    public partial class ForthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentTool");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentTool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentId = table.Column<int>(type: "int", nullable: false),
                    ToolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentTool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentTool_Rent_RentId",
                        column: x => x.RentId,
                        principalTable: "Rent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentTool_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentTool_RentId",
                table: "RentTool",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentTool_ToolId",
                table: "RentTool",
                column: "ToolId");
        }
    }
}
