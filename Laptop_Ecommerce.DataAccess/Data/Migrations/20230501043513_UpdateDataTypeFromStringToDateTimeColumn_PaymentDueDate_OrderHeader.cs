using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laptop_Ecommerce.DataAccess.Migrations
{
    public partial class UpdateDataTypeFromStringToDateTimeColumn_PaymentDueDate_OrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDueDate",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentDueDate",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
