using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class ChangePurchaseDayToPurchaseDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseDay",
                schema: "expenses",
                table: "Expenses",
                newName: "PurchaseDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                schema: "expenses",
                table: "Expenses",
                newName: "PurchaseDay");
        }
    }
}
