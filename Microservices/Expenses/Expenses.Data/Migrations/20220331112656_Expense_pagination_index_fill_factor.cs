using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Expense_pagination_index_fill_factor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expense_Pagination",
                schema: "expenses",
                table: "Expenses",
                column: "PurchaseDate")
                .Annotation("SqlServer:FillFactor", 80);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expense_Pagination",
                schema: "expenses",
                table: "Expenses");
        }
    }
}
