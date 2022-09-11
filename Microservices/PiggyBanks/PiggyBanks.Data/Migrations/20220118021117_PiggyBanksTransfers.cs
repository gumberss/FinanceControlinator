using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PiggyBanks.Data.Migrations
{
    public partial class PiggyBanksTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "piggyBanks",
                table: "PiggyBanks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "piggyBanks",
                table: "PiggyBanks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "piggyBanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_PiggyBanks_DestinationId",
                        column: x => x.DestinationId,
                        principalSchema: "piggyBanks",
                        principalTable: "PiggyBanks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transfers_PiggyBanks_SourceId",
                        column: x => x.SourceId,
                        principalSchema: "piggyBanks",
                        principalTable: "PiggyBanks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DestinationId",
                schema: "piggyBanks",
                table: "Transfers",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SourceId",
                schema: "piggyBanks",
                table: "Transfers",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "piggyBanks");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "piggyBanks",
                table: "PiggyBanks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "piggyBanks",
                table: "PiggyBanks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
