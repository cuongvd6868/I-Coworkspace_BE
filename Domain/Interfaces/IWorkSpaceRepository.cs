using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkSpaceRepository
    {
        Task<WorkSpace> GetWorkSpaceByIdAsync(int id);
        Task<IEnumerable<WorkSpace>> GetAllWorkSpacesAsync();
        Task<IEnumerable<WorkSpace>> GetWorkSpacesByTypeAsync(int typeId);
        Task<IEnumerable<WorkSpace>> GetWorkSpacesByLocationAsync(string location);
        Task<IEnumerable<WorkSpace>> GetFavoriteWorkSpacesAsync(string userId);
        Task AddWorkSpaceAsync(WorkSpace workSpace);
        Task UpdateWorkSpaceAsync(WorkSpace workSpace);
        Task DeleteWorkSpaceAsync(int id);
        Task<bool> IsFavoriteAsync(int workSpaceId, string userId);
        Task AddToFavoritesAsync(int workSpaceId, string userId);
        Task RemoveFromFavoritesAsync(int workSpaceId, string userId);
    }
}
