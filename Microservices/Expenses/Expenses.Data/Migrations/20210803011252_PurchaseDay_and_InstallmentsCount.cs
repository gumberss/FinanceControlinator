using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class PurchaseDay_and_InstallmentsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRecurrent",
                schema: "expenses",
                table: "Expenses",
                newName: "InstallmentsCount");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "expenses",
                table: "Expenses",
                newName: "PurchaseDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseDay",
                schema: "expenses",
                table: "Expenses",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "InstallmentsCount",
                schema: "expenses",
                table: "Expenses",
                newName: "IsRecurrent");
        }
    }
}
