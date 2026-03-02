using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Notification
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int SenderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SenderRole { get; set; }
    }
}
