using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
    public class SupportTicket
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public SupportTicketType TicketType { get; set; }
        public SupportTicketStatus Status { get; set; } = SupportTicketStatus.New;

        public string SubmittedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual AppUser SubmittedByUser { get; set; }
        public virtual List<SupportTicketReply> Replies { get; set; } = new();

        public string? AssignedToStaffId { get; set; }
        public virtual AppUser? AssignedToStaff { get; set; }
    }
}
