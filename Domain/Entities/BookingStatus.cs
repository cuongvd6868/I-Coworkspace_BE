using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class BookingStatus
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }
        public virtual List<Booking> Bookings { get; set; } = new();
    }
}
