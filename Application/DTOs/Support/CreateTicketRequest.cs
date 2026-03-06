using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Support
{
    public class CreateTicketRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int TicketType { get; set; } // Enum SupportTicketType
    }
}
