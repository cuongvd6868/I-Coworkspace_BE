using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Review
{
    public class CreateReviewRequest
    {
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
