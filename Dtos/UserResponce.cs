namespace Rems_Auth.Dtos
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string CountryCode { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool? IsAgent { get; set; }
        public int TotalListings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
