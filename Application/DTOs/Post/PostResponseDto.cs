using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Post
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ContentMarkdown { get; set; }

        public string? ContentHtml { get; set; }
        public string? ImageData { get; set; }
        public bool IsFeatured { get; set; }
    }
}
