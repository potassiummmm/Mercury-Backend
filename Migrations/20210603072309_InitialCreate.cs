using Microsoft.EntityFrameworkCore.Migrations;

namespace Mercury_Backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NickName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RealName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Major = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Credit = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Role = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Grade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AvatarId = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
