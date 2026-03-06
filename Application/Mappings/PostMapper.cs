using Application.DTOs.Post;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public static class PostMapper
    {
        public static PostResponseDto ToDto(this Post entity)
        {
            if (entity == null) return null;
            return new PostResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ContentMarkdown = entity.ContentMarkdown,
                ContentHtml = entity.ContentHtml,
                ImageData = entity.ImageData,
                IsFeatured = entity.IsFeatured
            };
        }

        public static Post ToPostCreateDTO(this CreatePostRequest entity)
        {
            if (entity == null) return null!;

            return new Post
            {
                Title = entity.Title,
                ContentMarkdown = entity.ContentMarkdown,
                ContentHtml = entity.ContentHtml,
                ImageData = entity.ImageData,
                IsFeatured = entity.IsFeatured,
                UserId = entity.UserId
            };
        }

        public static Post ToPostUpdateDTO(this UpdatePostRequest entity)
        {
            if (entity == null) return null!;

            return new Post
            {
                Title = entity.Title,
                ContentMarkdown = entity.ContentMarkdown,
                ContentHtml = entity.ContentHtml,
                ImageData = entity.ImageData,
                IsFeatured = entity.IsFeatured
            };
        }
    }
}
