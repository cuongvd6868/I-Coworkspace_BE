using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Amenity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? IconClass { get; set; }
        public virtual List<WorkSpaceRoomAmenity> WorkspaceRoomAmenities { get; set; } = new();
    }
}
