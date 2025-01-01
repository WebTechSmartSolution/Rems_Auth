namespace Rems_Auth.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ViewerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }


}
