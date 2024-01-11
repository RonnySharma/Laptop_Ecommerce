using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laptop_Ecommerce.DataAccess.Migrations
{
    public partial class Update_LaptopColumns_CompanyIdToLaptopCompanyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Laptops");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
