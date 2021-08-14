using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryWebApplication.Migrations
{
    public partial class AddedSalesInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductsJson = table.Column<string>(type: "TEXT", nullable: true),
                    SellTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Discount = table.Column<string>(type: "TEXT", nullable: true),
                    TotalPrice = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
