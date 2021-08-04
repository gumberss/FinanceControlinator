using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Data.Migrations
{
    public partial class Fix_Installment_as_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InstallmentsCount",
                schema: "expenses",
                table: "Expenses",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "InstallmentsCount",
                schema: "expenses",
                table: "Expenses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
