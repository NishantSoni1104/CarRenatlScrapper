using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalScrapper.Models
{
    public class Setting : BaseEntity
    {

        [StringLength(80)]
        [Required]
        public string PickupLocation { get; set; }
        [Required]
        public int  PickupDate { get; set; }
        [Required]
        public int ReturnDate { get; set; }
        [Required]
        public string DriverAge { get; set; }
        [Required]
        public string Country { get; set; }

        public string URL { get; set; }
    }
}
