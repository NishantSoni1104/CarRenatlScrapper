using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class SaveProcessProcessIntervalsInDateFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickupDate",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReturnDate",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "PickupDate",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
