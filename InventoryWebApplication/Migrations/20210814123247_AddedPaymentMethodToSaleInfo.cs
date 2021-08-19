using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryWebApplication.Migrations
{
    public partial class AddedPaymentMethodToSaleInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                table: "Sales",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_MethodId",
                table: "Sales",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_PaymentMethods_MethodId",
                table: "Sales",
                column: "MethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_PaymentMethods_MethodId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_MethodId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Sales");
        }
    }
}
