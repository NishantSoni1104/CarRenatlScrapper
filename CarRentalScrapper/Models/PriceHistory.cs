using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalScrapper.Models
{
    public class PriceHistory 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int ScrapeProcessId { get; set; }
       
        [StringLength(100)]
        [Required]
        public string Supplier { get; set; }
      
        [StringLength(150)]
        [Required]
        public string VehicleType { get; set; }
        
        [StringLength(80)]
        [Required]
        public string Vehicle { get; set; }
      
        [StringLength(40)]
        [Required]
        public string Transmission { get; set; }
        [Column(TypeName = "decimal(15, 2)")]
        [Required]
        public decimal TotalPrice { get; set; }
       
        [StringLength(40)]
        [Required]
        public string Currency { get; set; }
      
        [StringLength(150)]
        [Required]
        public string Logo { get; set; }
        [Required]
        public int NumberOfSeat { get; set; }
        
        [Required]
        public int NumberOfDoor { get; set; }
       
        [StringLength(35)]
        [Required]
        public string NumberOfBag { get; set; }
        
        [Column(TypeName = "decimal(15, 2)")]
        [Required]
        public decimal PerDayPrice { get; set; }
    
        [StringLength(35)]
        [Required]
        public string PaymentType { get; set; }


     
      
        public string VroomUrl { get; set; }

     
        public string ScreenShotUrl { get; set; }
        public virtual ScrapeProcess ScrapeProcess { get; set; }


    }
}
