using System.ComponentModel.DataAnnotations;

namespace Rems_Auth.Models
{
    public class UserUpdateRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string? CountryCode { get; } = "+92";

        public string? CurrentPassword { get; set; } // Optional for password change
        public string? NewPassword { get; set; }    // Optional for password change
        public IFormFile? ProfileImage { get; set; }
    }
}
