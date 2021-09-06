using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalScrapper.ViewModels
{
    public class SettingList
    {
        public int Id { get; set; }
        [StringLength(80)]
        [Required]
        public string PickupLocation { get; set; }
        [Required]
        public int PickupDate { get; set; }
        [Required]
        public int ReturnDate { get; set; }
    }
}
