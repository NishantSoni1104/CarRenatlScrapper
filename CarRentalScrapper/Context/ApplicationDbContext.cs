using CarRentalScrapper.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalScrapper.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasData(
                    new Setting { Id = 1, PickupDate = 1, ReturnDate = 2, PickupLocation = "Parramatta, New South Wales, Australia" },
                    new Setting { Id = 2, PickupDate = 1, ReturnDate = 2, PickupLocation = "Sydney Airport, New South Wales, Australia" },
                    new Setting { Id = 3, PickupDate = 1, ReturnDate = 2, PickupLocation = "Liverpool, New South Wales, Australia" },
                    new Setting { Id = 4, PickupDate = 1, ReturnDate = 2, PickupLocation = "Sutherland NSW, Australia" },
                    new Setting { Id = 5, PickupDate = 1, ReturnDate = 8, PickupLocation = "Parramatta, New South Wales, Australia" },
                    new Setting { Id = 6, PickupDate = 1, ReturnDate = 8, PickupLocation = "Sydney Airport, New South Wales, Australia" },
                    new Setting { Id = 7, PickupDate = 1, ReturnDate = 8, PickupLocation = "Liverpool, New South Wales, Australia" },
                    new Setting { Id = 8, PickupDate = 1, ReturnDate = 8, PickupLocation = "Sutherland NSW, Australia" },
                    new Setting { Id = 9, PickupDate = 1, ReturnDate = 8, PickupLocation = "Wollongong NSW, Australia" },
                    new Setting { Id = 10, PickupDate = 1, ReturnDate = 8, PickupLocation = "Penrith NSW, Australia" },
                    new Setting { Id = 11, PickupDate = 1, ReturnDate = 2, PickupLocation = "Wollongong NSW, Australia" },
                    new Setting { Id = 12, PickupDate = 1, ReturnDate = 2, PickupLocation = "Penrith NSW, Australia" }
                );

                entity
                    .Property(p => p.Country)
                    .HasDefaultValue("Australia");

                entity
                    .Property(p => p.DriverAge)
                    .HasDefaultValue("30 - 69");
            });
        }

        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<ScrapeProcess> ScrapeProcesses { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
