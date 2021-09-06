using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalScrapper.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScrapeProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeOfScrape = table.Column<DateTime>(nullable: false),
                    PickupDate = table.Column<int>(nullable: false),
                    ReturnDate = table.Column<int>(nullable: false),
                    PickupLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapeProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PickupLocation = table.Column<string>(maxLength: 80, nullable: false),
                    PickupDate = table.Column<int>(nullable: false),
                    ReturnDate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ScrapeProcessId = table.Column<int>(nullable: false),
                    Supplier = table.Column<string>(maxLength: 40, nullable: true),
                    Type = table.Column<string>(maxLength: 40, nullable: true),
                    Vehicle = table.Column<string>(nullable: true),
                    Transmission = table.Column<string>(maxLength: 40, nullable: true),
                    TotalPrice = table.Column<string>(maxLength: 40, nullable: true),
                    Currency = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceHistories_ScrapeProcesses_ScrapeProcessId",
                        column: x => x.ScrapeProcessId,
                        principalTable: "ScrapeProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "PickupDate", "PickupLocation", "ReturnDate" },
                values: new object[,]
                {
                    { 1, 1, "Parramatta, New South Wales, Australia", 2 },
                    { 2, 1, "Sydney Airport, New South Wales, Australia", 2 },
                    { 3, 1, "Liverpool, New South Wales, Australia", 2 },
                    { 4, 1, "Sutherland NSW, Australia", 2 },
                    { 5, 1, "Parramatta, New South Wales, Australia", 8 },
                    { 6, 1, "Sydney Airport, New South Wales, Australia", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistories_ScrapeProcessId",
                table: "PriceHistories",
                column: "ScrapeProcessId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistories");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "ScrapeProcesses");
        }
    }
}
