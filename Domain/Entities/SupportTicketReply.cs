using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class SupportTicketReply 
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }

        public int TicketId { get; set; }
        public virtual SupportTicket Ticket { get; set; }

        public string RepliedByUserId { get; set; }
        public virtual AppUser RepliedByUser { get; set; }
    }
}
