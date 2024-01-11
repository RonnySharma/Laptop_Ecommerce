using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laptop_Ecommerce.DataAccess.Migrations
{
    public partial class Add_Quantity_Active_ColumnsToLaptopModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Laptops_OrderDetails_OrderDetailsId",
            //    table: "Laptops");

            //migrationBuilder.DropIndex(
            //    name: "IX_Laptops_OrderDetailsId",
            //    table: "Laptops");

            //migrationBuilder.DropColumn(
            //    name: "OrderDetailsId",
            //    table: "Laptops");

            //migrationBuilder.AddColumn<bool>(
            //    name: "Active",
            //    table: "Laptops",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "Quantity",
            //    table: "Laptops",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Active",
            //    table: "Laptops");

            //migrationBuilder.DropColumn(
            //    name: "Quantity",
            //    table: "Laptops");

            //migrationBuilder.AddColumn<int>(
            //    name: "OrderDetailsId",
            //    table: "Laptops",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Laptops_OrderDetailsId",
            //    table: "Laptops",
            //    column: "OrderDetailsId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Laptops_OrderDetails_OrderDetailsId",
            //    table: "Laptops",
            //    column: "OrderDetailsId",
            //    principalTable: "OrderDetails",
            //    principalColumn: "Id");
        }
    }
}
