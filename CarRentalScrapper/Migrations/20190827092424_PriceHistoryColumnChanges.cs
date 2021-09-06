using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class PriceHistoryColumnChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PriceHistories",
                newName: "VehicleType");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Settings",
                nullable: false,
                defaultValue: "Australia");

            migrationBuilder.AddColumn<string>(
                name: "DriverAge",
                table: "Settings",
                nullable: false,
                defaultValue: "30 - 69");

            migrationBuilder.AlterColumn<string>(
                name: "Screenshot",
                table: "ScrapeProcesses",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ScrapeProcesses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverAge",
                table: "ScrapeProcesses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeat",
                table: "PriceHistories",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfDoor",
                table: "PriceHistories",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 35);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DriverAge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ScrapeProcesses");

            migrationBuilder.DropColumn(
                name: "DriverAge",
                table: "ScrapeProcesses");

            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "PriceHistories",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Screenshot",
                table: "ScrapeProcesses",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "ScrapeProcesses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfSeat",
                table: "PriceHistories",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfDoor",
                table: "PriceHistories",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
