using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Rems_Auth.Models
{
    public class AddListing
    {
        [Key]
        public Guid Id { get; set; }

        // Property Info
        
        public string PropertyName { get; set; }
        public string PropertyType {get ; set}
        public string CurrencyType {get ; set}
        public decimal SalePrice { get; set; }
        public decimal OfferPrice { get; set; }

        // Property Details
        public required string PropertyId { get; set; }
        public decimal PricePerSqft { get; set; }
        public int NoOfBedrooms { get; set; }
        public int NoOfBathrooms { get; set; }
        public int Sqft { get; set; }
        public int NoOfFloors { get; set; }
        public int GarageSize { get; set; }
        public int YearConstructed { get; set; }

        
       
        // Location
     
        public string Address { get; set; }
       
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string ZipCode { get; set; }

        // Property Gallery (Images)

        public Guid UserId { get; set; }
        public User User { get; set; }  // Navigation property to User

        
        public ICollection<Image> Images { get; set; }
        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relationship with User
       
    }
}
