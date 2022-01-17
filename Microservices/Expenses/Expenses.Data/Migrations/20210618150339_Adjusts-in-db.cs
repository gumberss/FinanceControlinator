using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Expenses.Data.Migrations
{
    public partial class Adjustsindb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                schema: "expenses",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                schema: "expenses",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "expenses",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "expenses",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                schema: "expenses",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                schema: "expenses",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "expenses",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                schema: "expenses",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "expenses",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                schema: "expenses",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                schema: "expenses",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "expenses",
                table: "Expenses");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "expenses",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
