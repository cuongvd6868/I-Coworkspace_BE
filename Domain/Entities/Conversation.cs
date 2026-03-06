using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Conversation
    {
        public int Id { get; set; }
        // ID của khách hàng
        public string CustomerId { get; set; }
        public virtual AppUser Customer { get; set; }

        // ID của chủ Workspace
        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public DateTime LastMessageAt { get; set; } = DateTime.Now;
        public virtual List<ChatMessage> Messages { get; set; } = new();
    }
}
