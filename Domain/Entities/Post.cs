using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(int.MaxValue)]
        public string? ContentMarkdown { get; set; }

        [MaxLength(int.MaxValue)]
        public string? ContentHtml { get; set; }
        [MaxLength(1000)]
        public string? ImageData { get; set; }
        public bool IsFeatured { get; set; }

        public string UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
