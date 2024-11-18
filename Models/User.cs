using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rems_Auth.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        public required  string CountryCode { get; set; }
        public required string MobileNumber { get; set; }
        public  bool? IsAgent { get; set; }
        public string? ResetToken { get; set; } // New property
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? RefreshToken { get; set; }  // New property
        public DateTime? RefreshTokenExpires { get; set; }  // New property

        // Relationship with Listing
        public ICollection<AddListing> Listings { get; set; } = new List<AddListing>();

    }
}
