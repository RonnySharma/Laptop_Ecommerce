using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laptop_Ecommerce.DataAccess.Migrations
{
    public partial class Activcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Laptops",
                type: "bit",
                nullable: false,
                defaultValue: true);
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Laptops");

        }
    }
}
