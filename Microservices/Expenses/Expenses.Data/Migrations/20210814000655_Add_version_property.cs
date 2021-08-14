using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Add_version_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "expenses",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "expenses",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "expenses",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "expenses",
                table: "ExpenseItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "expenses",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "expenses",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "expenses",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "expenses",
                table: "ExpenseItems");
        }
    }
}
