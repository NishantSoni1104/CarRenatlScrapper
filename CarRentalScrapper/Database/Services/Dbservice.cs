using CarRentalScrapper.Context;
using CarRentalScrapper.Database.IServices;
using CarRentalScrapper.Models;
using CarRentalScrapper.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalScrapper.Database.Services
{
    public class Dbservice : IDbservice
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Dbservice(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task AddPriceHistories(List<PriceHistoryDto> result)
        {
            try
            {
                foreach (var i in result)
                {
                    await _context.PriceHistories.AddAsync(new PriceHistory
                    {
                        ScrapeProcessId = i.ScrapeProcessId,
                        Vehicle = i.vehicle,
                        TotalPrice = i.totalPrice,
                        VehicleType = i.type,
                        Transmission = i.transmission,
                        Logo = i.logo,
                        Supplier = i.Supplier,
                        NumberOfSeat = i.numberOfSeat,
                        NumberOfDoor = i.numberOfDoor,
                        NumberOfBag = i.numberOfBag,
                        PerDayPrice = i.perDayPrice,
                        PaymentType = i.paymentType,
                        Currency = i.currency,
                        VroomUrl = i.detailUrl,
                        ScreenShotUrl = Path.Combine("104.200.137.184", "ScreenShot", DateTime.Now.ToString("MM-dd-yyyy"), i.vehicle+i.ScrapeProcessId + ".png")
                });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Settingsdto>> AllSettings()
        {
            var date = DateTime.Now.Date;
            var time = new DateTime(01,01,01,10, 00, 00);
            var combinedDateTime = date.AddTicks(time.TimeOfDay.Ticks);
            var setting = await _context.Settings
                 .AsNoTracking()
                 .Select(i => new Settingsdto
                 {
                     Id = i.Id,
                     PickupLocation = i.PickupLocation,
                     PickupDateInterval = i.PickupDate,
                     ReturnDateInterval = i.ReturnDate,
                     PickupDate = combinedDateTime.AddDays(i.PickupDate),
                     ReturnDate = combinedDateTime.AddDays(i.ReturnDate),
                     Country = i.Country,
                     DriverAge = i.DriverAge,
                     URL = i.URL
                 })
                 .ToListAsync();

            return setting;
        }

        public async Task<bool> IsScrapeProcessExecuted(Settingsdto setting)
        {
            return !(await _context.ScrapeProcesses
                        .AsNoTracking()
                        .AnyAsync(i =>
                            i.PickupDate == setting.PickupDate &&
                            i.PickupLocation == setting.PickupLocation &&
                            i.ReturnDate == setting.ReturnDate &&
                            i.TimeOfScrape.Date == DateTime.Now.Date &&
                            i.Country == setting.Country &&
                            i.DriverAge == setting.DriverAge));
        }

        public async Task<int> AddScrapeProcesses(Settingsdto setting)
        {
            string localPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ScreenShot", DateTime.Now.ToString("MM-dd-yyyy"), setting.Id + ".png");
           
           string httpPath = Path.Combine("104.200.137.184", "ScreenShot", DateTime.Now.ToString("MM-dd-yyyy"), setting.Id + ".png");
            var scrapeProcess = new ScrapeProcess
            {
                PickupLocation = setting.PickupLocation,
                PickupDate = setting.PickupDate,
                ReturnDate = setting.ReturnDate,
                TimeOfScrape = DateTime.Now,
                
                Country = setting.Country,
                DriverAge = setting.DriverAge,
                Interval = setting.ReturnDateInterval,
               
            };
            await _context.ScrapeProcesses.AddAsync(scrapeProcess);
            await _context.SaveChangesAsync();
            return scrapeProcess.Id;
        }
    }
}
