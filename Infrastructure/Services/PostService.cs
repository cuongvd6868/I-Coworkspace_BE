using Application.DTOs.Post;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
        {
            var entities = await _postRepository.GetAllPostsAsync();
            return entities.Select(e => e.ToDto());
        }

        public async Task<IEnumerable<PostResponseDto>> GetFeaturedPostsAsync()
        {
            var entities = await _postRepository.GetFeaturedPostsAsync();
            return entities.Select(e => e.ToDto());
        }

        public async Task<PostResponseDto> GetPostByIdAsync(int id)
        {
            var entity = await _postRepository.GetPostByIdAsync(id);
            return entity.ToDto(); 
        }

        public async Task CreatePostAsync(Post post)
        {
            await _postRepository.AddPostAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            var existing = await _postRepository.GetPostByIdAsync(post.Id);
            if (existing == null) throw new KeyNotFoundException("Không tìm thấy bài đăng");

            existing.Title = post.Title;
            existing.ContentMarkdown = post.ContentMarkdown;
            existing.ContentHtml = post.ContentHtml;
            existing.ImageData = post.ImageData;
            existing.IsFeatured = post.IsFeatured;

            await _postRepository.UpdatePostAsync(existing);
        }

        public async Task DeletePostAsync(int id)
        {
            await _postRepository.DeletePostAsync(id);
        }
    }
}