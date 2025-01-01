namespace Rems_Auth.Dtos
{
    public class SendMessageRequest
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
    }

}
