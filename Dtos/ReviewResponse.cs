namespace Rems_Auth.Dtos
{
    public class ReviewResponse
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
