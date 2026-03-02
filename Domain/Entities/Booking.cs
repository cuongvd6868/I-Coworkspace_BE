using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Booking
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string BookingCode { get; set; }

        public string? CustomerId { get; set; }
        public int? GuestId { get; set; }
        public int WorkSpaceRoomId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        public int NumberOfParticipants { get; set; } = 1;

        [MaxLength(1000)]
        public string? SpecialRequests { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal FinalAmount { get; set; }
        [MaxLength(255)]
        public string? PaymentTransactionId { get; set; }
        [MaxLength(3)]
        public string? Currency { get; set; } = "VND";

        public int BookingStatusId { get; set; }

        public DateTime? CheckedInAt { get; set; }
        public DateTime? CheckedOutAt { get; set; }

        [MaxLength(500)]
        public string? CancellationReason { get; set; }
        public bool IsReviewed { get; set; } = false;
        public int? PaymentMethodID { get; set; }

        public virtual AppUser? Customer { get; set; }
        public virtual Guest? Guest { get; set; }
        public virtual WorkSpaceRoom? WorkSpaceRoom { get; set; }
        public virtual BookingStatus? BookingStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public virtual List<BookingParticipant> BookingParticipants { get; set; } = new();
        public virtual List<Review> Reviews { get; set; } = new();

    }
}
