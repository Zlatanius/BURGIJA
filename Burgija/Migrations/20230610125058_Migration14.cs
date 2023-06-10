using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Migrations
{
    public partial class Migration14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_Discount_DiscountId",
                table: "Rent");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Rent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_Discount_DiscountId",
                table: "Rent",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rent_Discount_DiscountId",
                table: "Rent");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountId",
                table: "Rent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rent_Discount_DiscountId",
                table: "Rent",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
