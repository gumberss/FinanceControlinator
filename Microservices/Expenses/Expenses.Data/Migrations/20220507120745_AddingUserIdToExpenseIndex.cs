using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.Data.Migrations
{
    public partial class AddingUserIdToExpenseIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Expense_UserId",
                schema: "expenses",
                table: "Expenses",
                column: "UserId")
                .Annotation("SqlServer:FillFactor", 80);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expense_UserId",
                schema: "expenses",
                table: "Expenses");
        }
    }
}
