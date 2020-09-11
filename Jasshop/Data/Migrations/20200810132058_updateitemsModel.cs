using Microsoft.EntityFrameworkCore.Migrations;

namespace Jasshop.Data.Migrations
{
    public partial class updateitemsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Items");
        }
    }
}
