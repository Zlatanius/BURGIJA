using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Rating_RatingId",
                table: "Review");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Review_RatingId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Review");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Review",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Review");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Review_RatingId",
                table: "Review",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Rating_RatingId",
                table: "Review",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
