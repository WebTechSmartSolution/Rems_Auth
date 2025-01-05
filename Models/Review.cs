namespace Rems_Auth.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string name { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } // Rating scale, e.g., 1-5
        public DateTime CreatedAt { get; set; }
    }

}
