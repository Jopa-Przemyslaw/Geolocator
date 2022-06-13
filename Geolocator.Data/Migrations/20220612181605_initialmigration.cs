using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Geolocator.Data.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpInfos",
                columns: table => new
                {
                    Ip = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Loc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Org = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpInfos", x => x.Ip);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpInfos");
        }
    }
}
