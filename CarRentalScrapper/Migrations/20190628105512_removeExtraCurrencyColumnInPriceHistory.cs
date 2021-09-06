using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class removeExtraCurrencyColumnInPriceHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currrency",
                table: "PriceHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currrency",
                table: "PriceHistories",
                maxLength: 35,
                nullable: false,
                defaultValue: "");
        }
    }
}
