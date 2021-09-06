using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Settings",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotURL",
                table: "ScrapeProcesses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PriceHistories",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 9, 1, "Wollongong NSW, Australia", 8 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 10, 1, "Penrith NSW, Australia", 8 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 11, 1, "Wollongong NSW, Australia", 2 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[] { 12, 1, "Penrith NSW, Australia", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "ScreenshotURL",
                table: "ScrapeProcesses");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Settings",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ScrapeProcesses",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PriceHistories",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
