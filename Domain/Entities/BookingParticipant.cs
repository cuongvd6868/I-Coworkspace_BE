using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class BookingParticipant 
    {
        [Required]
        public int Id { get; set; }
        public int BookingId { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }


        public virtual Booking? Booking { get; set; }
    }
}
