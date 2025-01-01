namespace Rems_Auth.Dtos
{
    public class MessageDto
    {
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
