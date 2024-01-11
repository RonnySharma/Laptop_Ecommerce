using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laptop_Ecommerce.DataAccess.Migrations
{
    public partial class AddLaptopTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LaptopCompanyId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessorId = table.Column<int>(type: "int", nullable: false),
                    GraphicsCardId = table.Column<int>(type: "int", nullable: false),
                    RamMemory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageSpace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price10 = table.Column<double>(type: "float", nullable: false),
                    ListPrice20 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laptops_GraphicsCards_GraphicsCardId",
                        column: x => x.GraphicsCardId,
                        principalTable: "GraphicsCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Laptops_LaptopCompanies_LaptopCompanyId",
                        column: x => x.LaptopCompanyId,
                        principalTable: "LaptopCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Laptops_Processors_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Processors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_GraphicsCardId",
                table: "Laptops",
                column: "GraphicsCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_LaptopCompanyId",
                table: "Laptops",
                column: "LaptopCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_ProcessorId",
                table: "Laptops",
                column: "ProcessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laptops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GraphicsCards",
                table: "GraphicsCards");

            migrationBuilder.RenameTable(
                name: "GraphicsCards",
                newName: "Graphics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Graphics",
                table: "Graphics",
                column: "Id");
        }
    }
}
