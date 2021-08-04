using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Change_insertDate_for_create_date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsertDate",
                schema: "expenses",
                table: "Items",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                schema: "expenses",
                table: "Expenses",
                newName: "CreatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "expenses",
                table: "Items",
                newName: "InsertDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "expenses",
                table: "Expenses",
                newName: "InsertDate");
        }
    }
}
