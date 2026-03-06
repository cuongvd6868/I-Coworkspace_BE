using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        public string SenderId { get; set; }
        public virtual AppUser Sender { get; set; } 

        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
