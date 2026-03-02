using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Guest
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = default!;
        [Required, MaxLength(100)]
        public string LastName { get; set; } = default!;

        [Required, MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public virtual List<Booking> Bookings { get; set; } = new();
    }
}
