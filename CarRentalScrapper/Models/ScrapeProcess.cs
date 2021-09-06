using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentalScrapper.Models
{
    public class ScrapeProcess : BaseEntity
    {
        [Required]
        public DateTime TimeOfScrape { get; set; }
        [Required]
        public DateTime PickupDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public int Interval { get; set; }
        [Required]
        public string PickupLocation { get; set; }
   
        [Required]
        public string DriverAge { get; set; }
        [Required]
        public string Country { get; set; }
       
        public virtual List<PriceHistory> PriceHistory { get; set; }
    }
}
