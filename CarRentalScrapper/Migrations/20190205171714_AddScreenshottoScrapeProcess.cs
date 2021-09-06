using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class AddScreenshottoScrapeProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Screenshot",
                table: "ScrapeProcesses",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Screenshot",
                table: "ScrapeProcesses");
        }
    }
}
