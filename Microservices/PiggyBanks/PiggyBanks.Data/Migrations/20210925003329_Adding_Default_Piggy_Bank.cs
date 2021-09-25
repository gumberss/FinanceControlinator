using Microsoft.EntityFrameworkCore.Migrations;

namespace PiggyBanks.Data.Migrations
{
    public partial class Adding_Default_Piggy_Bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                schema: "piggyBanks",
                table: "PiggyBanks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                schema: "piggyBanks",
                table: "PiggyBanks");
        }
    }
}
