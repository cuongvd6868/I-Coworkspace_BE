using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Booking
{
    public class CreateBookingRequest
    {
        public int WorkSpaceRoomId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public int NumberOfParticipants { get; set; }
        public string? SpecialRequests { get; set; }
        public int? PaymentMethodID { get; set; }
    }
}
