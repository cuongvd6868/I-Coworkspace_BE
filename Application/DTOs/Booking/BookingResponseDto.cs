using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string BookingCode { get; set; } = null!;
        public string? RoomName { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public decimal FinalAmount { get; set; }
        public string? StatusName { get; set; }
        public bool IsReviewed { get; set; }
    }
}
