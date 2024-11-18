using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rems_Auth.Models
{
    public class AddListing
    {
        [Key]
        public int Id { get; set; }

        // Property Info
        [Required]
        public string PropertyName { get; set; }
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

        // Description
        public string Title { get; set; }
        public string TotalArea { get; set; }
        public string Description { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int GarageSizeDescription { get; set; }
        public int YearBuilt { get; set; }

        // Location
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }

        // Property Gallery (Images)
        [NotMapped] // To exclude from DB, we can handle this as a separate entity or in blob storage.
        public ICollection<string> Images { get; set; } = new List<string>();

        // Property Documents
        [NotMapped]
        public string Document { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relationship with User
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public  User User { get; set; }
    }
}
