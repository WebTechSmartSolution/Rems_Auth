using System;

namespace Rems_Auth.Models
{
    public class Message
    {
        public Guid Id { get; set; } 
        public Guid ChatId { get; set; } 
        public Guid SenderId { get; set; } 
        public Guid ReceiverId { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual User Sender { get; set; } 
        public virtual User Receiver { get; set; } 
        public virtual Chat Chat { get; set; } 

    }
}
