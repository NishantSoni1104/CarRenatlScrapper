﻿// <auto-generated />
using System;
using CarRentalScrapper.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRentalScrapper.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190205171714_AddScreenshottoScrapeProcess")]
    partial class AddScreenshottoScrapeProcess
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CarRentalScrapper.Models.PriceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Currency")
                        .HasMaxLength(40);

                    b.Property<int>("ScrapeProcessId");

                    b.Property<string>("Supplier")
                        .HasMaxLength(40);

                    b.Property<string>("TotalPrice")
                        .HasMaxLength(40);

                    b.Property<string>("Transmission")
                        .HasMaxLength(40);

                    b.Property<string>("Type")
                        .HasMaxLength(40);

                    b.Property<string>("Vehicle");

                    b.HasKey("Id");

                    b.HasIndex("ScrapeProcessId");

                    b.ToTable("PriceHistories");
                });

            modelBuilder.Entity("CarRentalScrapper.Models.ScrapeProcess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PickupDate");

                    b.Property<string>("PickupLocation");

                    b.Property<int>("ReturnDate");

                    b.Property<string>("Screenshot")
                        .HasMaxLength(250);

                    b.Property<DateTime>("TimeOfScrape");

                    b.HasKey("Id");

                    b.ToTable("ScrapeProcesses");
                });

            modelBuilder.Entity("CarRentalScrapper.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PickupDate");

                    b.Property<string>("PickupLocation")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<int>("ReturnDate");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasData(
                        new { Id = 1, PickupDate = 1, PickupLocation = "Parramatta, New South Wales, Australia", ReturnDate = 2 },
                        new { Id = 2, PickupDate = 1, PickupLocation = "Sydney Airport, New South Wales, Australia", ReturnDate = 2 },
                        new { Id = 3, PickupDate = 1, PickupLocation = "Liverpool, New South Wales, Australia", ReturnDate = 2 },
                        new { Id = 4, PickupDate = 1, PickupLocation = "Sutherland NSW, Australia", ReturnDate = 2 },
                        new { Id = 5, PickupDate = 1, PickupLocation = "Parramatta, New South Wales, Australia", ReturnDate = 8 },
                        new { Id = 6, PickupDate = 1, PickupLocation = "Sydney Airport, New South Wales, Australia", ReturnDate = 8 }
                    );
                });

            modelBuilder.Entity("CarRentalScrapper.Models.PriceHistory", b =>
                {
                    b.HasOne("CarRentalScrapper.Models.ScrapeProcess", "ScrapeProcess")
                        .WithMany("PriceHistory")
                        .HasForeignKey("ScrapeProcessId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
