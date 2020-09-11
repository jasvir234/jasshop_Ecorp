using Microsoft.EntityFrameworkCore.Migrations;

namespace Jasshop.Data.Migrations
{
    public partial class Updatesales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Buyer",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Sales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buyer",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Sales");
        }
    }
}
