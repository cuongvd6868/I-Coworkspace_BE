using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Post
{
    public class CreatePostRequest
    {
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
    }
}
