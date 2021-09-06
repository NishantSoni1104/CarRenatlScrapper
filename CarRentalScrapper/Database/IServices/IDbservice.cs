using CarRentalScrapper.Database.Services;
using CarRentalScrapper.Models;
using CarRentalScrapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalScrapper.Database.IServices
{
    public interface IDbservice 
    {
        Task AddPriceHistories(List<PriceHistoryDto> result);
        Task<List<Settingsdto>> AllSettings();
        Task<bool> IsScrapeProcessExecuted(Settingsdto setting);
        Task<int> AddScrapeProcesses(Settingsdto setting);
    }
}
