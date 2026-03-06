using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Chat
{
    public class ConversationDto
    {
        public int Id { get; set; }

        // Thông tin đối phương (nếu mình là Customer thì đây là Owner và ngược lại)
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string? OtherUserAvatar { get; set; }

        public string LastMessage { get; set; }
        public DateTime LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}