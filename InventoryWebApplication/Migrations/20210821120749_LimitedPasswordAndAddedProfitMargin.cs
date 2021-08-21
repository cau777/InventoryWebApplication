using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryWebApplication.Migrations
{
    public partial class LimitedPasswordAndAddedProfitMargin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfitMarginPercentage",
                table: "PaymentMethods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfitMarginPercentage",
                table: "PaymentMethods");
        }
    }
}
