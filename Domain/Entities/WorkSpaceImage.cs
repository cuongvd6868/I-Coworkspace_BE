using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceImage
    {
        public int Id { get; set; }
        public int WorkSpaceId { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string ImageUrl { get; set; }

        [MaxLength(255)]
        public string? Caption { get; set; }


        public virtual WorkSpace? WorkSpace { get; set; }
    }
}
