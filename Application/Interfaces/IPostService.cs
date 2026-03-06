using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.Post;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
        Task<IEnumerable<PostResponseDto>> GetFeaturedPostsAsync(); 
        Task<PostResponseDto> GetPostByIdAsync(int id);
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
    }
}
