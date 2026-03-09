using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? UserName { get; set; }
        public string? RoomName { get; set; }
        public bool IsVerified { get; set; }
    }
}
