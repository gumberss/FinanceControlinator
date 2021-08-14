using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Change_Invoice_to_invoies_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoice_InvoiceId",
                schema: "expenses",
                table: "InvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                schema: "expenses",
                table: "Invoice");

            migrationBuilder.RenameTable(
                name: "Invoice",
                schema: "expenses",
                newName: "Invoices",
                newSchema: "expenses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                schema: "expenses",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                schema: "expenses",
                table: "InvoiceItems",
                column: "InvoiceId",
                principalSchema: "expenses",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                schema: "expenses",
                table: "InvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                schema: "expenses",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                schema: "expenses",
                newName: "Invoice",
                newSchema: "expenses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                schema: "expenses",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoice_InvoiceId",
                schema: "expenses",
                table: "InvoiceItems",
                column: "InvoiceId",
                principalSchema: "expenses",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
