using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class HostProfile
    {
        [Required]
        public int Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(255)]
        public string? CompanyName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(1000)]
        public string? Avatar { get; set; }

        [MaxLength(1000)]
        public string? CoverPhoto { get; set; }
        public bool IsVerified { get; set; } = false;
        public virtual AppUser? User { get; set; }
        public virtual List<WorkSpace> Workspaces { get; set; } = new();
        public virtual List<Promotion> Promotions { get; set; } = new();

    }
}
