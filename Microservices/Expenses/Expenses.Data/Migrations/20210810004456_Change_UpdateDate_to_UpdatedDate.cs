using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Change_UpdateDate_to_UpdatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "InvoiceItems",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "Invoice",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "Expenses",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "ExpenseItems",
                newName: "UpdatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "expenses",
                table: "InvoiceItems",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "expenses",
                table: "Invoice",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "expenses",
                table: "Expenses",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "expenses",
                table: "ExpenseItems",
                newName: "UpdateDate");
        }
    }
}
