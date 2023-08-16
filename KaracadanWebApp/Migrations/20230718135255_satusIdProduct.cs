using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaracadanWebApp.Migrations
{
    public partial class satusIdProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Products");
        }
    }
}
