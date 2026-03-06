using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Support
{
    public class TicketResponseDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TicketType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string SubmittedByUserName { get; set; } = string.Empty;

        // Danh sách các phản hồi bên trong
        public List<ReplyResponseDto> Replies { get; set; } = new();
    }
}
