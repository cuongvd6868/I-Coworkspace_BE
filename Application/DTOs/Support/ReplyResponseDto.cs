using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Support
{
    public class ReplyResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string RepliedByUserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
