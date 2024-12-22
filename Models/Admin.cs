using System.ComponentModel.DataAnnotations;

namespace Rems_Auth.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; } // Unique identifier for the admin
        public string Username { get; set; } // Admin username
        public string PasswordHash { get; set; } // Hashed password
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp
        public DateTime? UpdatedAt { get; set; } // Last update timestamp
    }
}
