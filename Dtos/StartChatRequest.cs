namespace Rems_Auth.Dtos
{
    public class StartChatRequest
    {
        public Guid ListingId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ViewerId { get; set; }
    }

}