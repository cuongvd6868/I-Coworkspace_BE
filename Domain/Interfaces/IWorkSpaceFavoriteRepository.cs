using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IWorkSpaceFavoriteRepository
    {
        Task<bool> IsFavoriteAsync(int workSpaceId, string userId);
        Task AddToFavoritesAsync(int workSpaceId, string userId);
        Task RemoveFromFavoritesAsync(int workSpaceId, string userId);
        Task<IEnumerable<int>> GetWorkSpaceIdsByUserIdAsync(string userId);
    }
}
