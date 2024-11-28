namespace Rems_Auth.Dtos
{
    public class ListingRequest
    {
        // Property Info
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string CurrencyType { get; set; }
        public decimal SalePrice { get; set; }
        public decimal OfferPrice { get; set; }

        // Property Details
        public string PropertyId { get; set; }
        public decimal PricePerSqft { get; set; }
        public int NoOfBedrooms { get; set; }
        public int NoOfBathrooms { get; set; }
        public int Sqft { get; set; }
        public int NoOfFloors { get; set; }
        public int GarageSize { get; set; }
        public int YearConstructed { get; set; }

        public string Email { get; set; }
        public string Phone {get; set;}

        // Location
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<IFormFile> Images { get; set; }

       
    }

}
