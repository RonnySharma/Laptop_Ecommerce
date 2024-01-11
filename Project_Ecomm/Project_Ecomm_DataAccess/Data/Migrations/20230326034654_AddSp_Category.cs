using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Ecomm_DataAccess.Migrations
{
    public partial class AddSp_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE CreateCategory

                                 @name varchar(50)
                                 AS
                                 insert categories values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateCategory
                                 @id int,
                                 @name varchar(50)
                                 AS
                                 Update categories set name=@name where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteCategory
                                 @id int
                                 AS
                                 Delete from categories where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCategories
                                 AS
                                 select * from categories");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCategory
                                 @id int
                                 AS
                                 select * from categories where id=@id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
