using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string UserId { get; set; }
        public int WorkSpaceRoomId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }


        public bool IsVerified { get; set; } = false;
        public bool IsPublic { get; set; } = true;
        public virtual Booking? Booking { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual WorkSpaceRoom? WorkSpaceRoom { get; set; }
    }
}
