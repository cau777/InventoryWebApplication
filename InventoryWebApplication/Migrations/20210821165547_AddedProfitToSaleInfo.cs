using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryWebApplication.Migrations
{
    public partial class AddedProfitToSaleInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Profit",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profit",
                table: "Sales");
        }
    }
}
