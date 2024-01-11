using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Ecomm_DataAccess.Migrations
{
    public partial class AddSp_CoverType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE CreateCoverType
                                 @name varchar(50)
                                 AS
                                 insert coverTypes values(@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateCoverType
                                 @id int, 
                                 @name varchar(50)
                                 AS
                                 Update coverTypes set name=@name where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteCoverType
                                 @id Int
                                 AS
                                 Delete from coverTypes where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCovertTypes
                                 AS
                                 select * from coverTypes");
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverType
                                 @id int
                                 AS
                                 select *from coverTypes where id=@id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
