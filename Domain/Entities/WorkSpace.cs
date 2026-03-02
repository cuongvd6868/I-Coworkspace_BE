using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpace 
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int HostId { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public int? WorkSpaceTypeId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;


        public virtual Address? Address { get; set; }
        public virtual HostProfile? Host { get; set; }
        public virtual List<WorkSpaceImage> WorkSpaceImages { get; set; } = new();
        public virtual WorkSpaceType? WorkSpaceType { get; set; }
        public virtual List<WorkSpaceRoom> WorkSpaceRooms { get; set; } = new();
        public virtual List<WorkSpaceFavorite> WorkSpaceFavorites { get; set; } = new();
    }
}
