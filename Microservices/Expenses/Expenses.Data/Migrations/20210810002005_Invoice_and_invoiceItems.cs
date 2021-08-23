using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Invoice_and_invoiceItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Expenses_ExpenseId",
                schema: "expenses",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                schema: "expenses",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "expenses",
                newName: "ExpenseItems",
                newSchema: "expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ExpenseId",
                schema: "expenses",
                table: "ExpenseItems",
                newName: "IX_ExpenseItems_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseItems",
                schema: "expenses",
                table: "ExpenseItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                schema: "expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstallmentCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "expenses",
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                schema: "expenses",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                schema: "expenses",
                table: "ExpenseItems",
                column: "ExpenseId",
                principalSchema: "expenses",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                schema: "expenses",
                table: "ExpenseItems");

            migrationBuilder.DropTable(
                name: "InvoiceItems",
                schema: "expenses");

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseItems",
                schema: "expenses",
                table: "ExpenseItems");

            migrationBuilder.RenameTable(
                name: "ExpenseItems",
                schema: "expenses",
                newName: "Items",
                newSchema: "expenses");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseItems_ExpenseId",
                schema: "expenses",
                table: "Items",
                newName: "IX_Items_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                schema: "expenses",
                table: "Items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Expenses_ExpenseId",
                schema: "expenses",
                table: "Items",
                column: "ExpenseId",
                principalSchema: "expenses",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
