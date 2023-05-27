using Microsoft.EntityFrameworkCore.Migrations;

namespace Burgija.Data.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Rent_RentId",
                table: "Delivery");

            migrationBuilder.AlterColumn<int>(
                name: "RentId",
                table: "Delivery",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Rent_RentId",
                table: "Delivery",
                column: "RentId",
                principalTable: "Rent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Rent_RentId",
                table: "Delivery");

            migrationBuilder.AlterColumn<int>(
                name: "RentId",
                table: "Delivery",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Rent_RentId",
                table: "Delivery",
                column: "RentId",
                principalTable: "Rent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
