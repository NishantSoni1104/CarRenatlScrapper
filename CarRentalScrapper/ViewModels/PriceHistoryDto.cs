namespace CarRentalScrapper.ViewModels
{
    public class PriceHistoryDto
    {
        public int ScrapeProcessId { get; set; }
        public string vehicle { get; set; }
        public decimal totalPrice { get; set; }
        public string type { get; set; }
        public string transmission { get; set; }
        public string logo { get; set; }
        public string Supplier { get; set; }
        public int numberOfSeat { get; set; }
        public int numberOfDoor { get; set; }
        public string numberOfBag { get; set; }
        public decimal perDayPrice { get; set; }
        public string paymentType { get; set; }                            
        public string currency { get; set; }

        public string detailUrl { get; set; }

    }
}
