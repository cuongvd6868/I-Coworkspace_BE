using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class WorkSpaceFavoriteService : IWorkSpaceFavoriteService
    {
        private readonly IWorkSpaceFavoriteRepository _favoriteRepository;

        public WorkSpaceFavoriteService(IWorkSpaceFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task ToggleFavoriteAsync(int workSpaceId, string userId)
        {
            var isFavorite = await _favoriteRepository.IsFavoriteAsync(workSpaceId, userId);

            if (isFavorite)
            {
                await _favoriteRepository.RemoveFromFavoritesAsync(workSpaceId, userId);
            }
            else
            {
                await _favoriteRepository.AddToFavoritesAsync(workSpaceId, userId);
            }
        }
    }
}