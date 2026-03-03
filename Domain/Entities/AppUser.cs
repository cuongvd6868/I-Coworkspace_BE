using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Schema.InverseProperty("Customer")]
        public virtual List<Booking> Bookings { get; set; } = new();
        public virtual HostProfile? HostProfile { get; set; }
        public virtual List<Review> Reviews { get; set; } = new();
        public virtual List<WorkSpaceFavorite> WorkSpaceFavorites { get; set; } = new();
        public virtual List<Post> Posts { get; set; } = new();
        public virtual List<SupportTicket> SupportTickets { get; set; } = new();
    }
}
