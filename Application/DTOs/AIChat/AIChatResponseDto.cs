using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.AIChat
{
    public class AIChatResponseDto
    {
        public string Answer { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
