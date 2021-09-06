using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class PriceHistory_ConvertStringsToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "PriceHistories",
                type: "decimal(15, 2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<decimal>(
                name: "PerDayPrice",
                table: "PriceHistories",
                type: "decimal(15, 2)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 35);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 7, 1, "Liverpool, New South Wales, Australia", 8 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 8, 1, "Sutherland NSW, Australia", 8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AlterColumn<string>(
                name: "TotalPrice",
                table: "PriceHistories",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15, 2)");

            migrationBuilder.AlterColumn<string>(
                name: "PerDayPrice",
                table: "PriceHistories",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15, 2)");
        }
    }
}
