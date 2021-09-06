using CarRentalScrapper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalScrapper.ViewModels
{
    public class Settingsdto
    {
        public Settingsdto()
        {
            GridSetting = new List<SettingList>();

        }
        public int Id { get; set; }
        public List<SettingList> GridSetting { get; set; }
        [StringLength(80)]
        [Required]
        public string PickupLocation { get; set; }
        [Required]
        public DateTime PickupDate { get; set; }
        public int PickupDateInterval { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public int ReturnDateInterval { get; set; }
        public string DriverAge { get; set; }
        public string Country { get; set; }
        public string URL { get; set; }
    }
}
