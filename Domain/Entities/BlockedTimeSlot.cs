
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class BlockedTimeSlot
    {
        [Required]
        public int Id { get; set; }
        public int WorkSpaceRoomId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual WorkSpaceRoom? WorkSpaceRoom { get; set; }
    }
}
