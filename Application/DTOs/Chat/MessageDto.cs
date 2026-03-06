using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Chat
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}