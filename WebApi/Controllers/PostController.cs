using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Extensions;
using Application.DTOs.Post;
using Application.Mappings;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedPosts()
        {
            var posts = await _postService.GetFeaturedPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                request.UserId = User.GetUserId();

                var post = request.ToPostCreateDTO();
                await _postService.CreatePostAsync(post);

                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, new { Message = "Post created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request)
        {
            try
            {
                var post = request.ToPostUpdateDTO();
                await _postService.UpdatePostAsync(post);
                return Ok(new { Message = "Post updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}