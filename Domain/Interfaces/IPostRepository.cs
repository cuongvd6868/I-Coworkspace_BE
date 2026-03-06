using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<Post>> GetFeaturedPostsAsync();

        Task<Post> GetPostByIdAsync(int id);

        Task AddPostAsync(Post post);

        Task UpdatePostAsync(Post post);

        Task DeletePostAsync(int id);

    }
}