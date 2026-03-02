using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceType
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }


        public virtual List<WorkSpace> Workspaces { get; set; } = new();
    }
}
