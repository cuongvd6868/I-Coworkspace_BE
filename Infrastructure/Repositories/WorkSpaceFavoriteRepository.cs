using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class WorkSpaceFavoriteRepository : IWorkSpaceFavoriteRepository
    {
        private readonly AppDbContext _context;

        public WorkSpaceFavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToFavoritesAsync(int workSpaceId, string userId)
        {
            var favorite = new WorkSpaceFavorite
            {
                UserId = userId,
                WorkspaceId = workSpaceId
            };
            _context.WorkSpaceFavorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFavoriteAsync(int workSpaceId, string userId)
        {
            return await _context.WorkSpaceFavorites
                .AnyAsync(f => f.UserId == userId && f.WorkspaceId == workSpaceId);
        }

        public async Task RemoveFromFavoritesAsync(int workSpaceId, string userId)
        {
            var favorite = await _context.WorkSpaceFavorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.WorkspaceId == workSpaceId);
            if (favorite != null)
            {
                _context.WorkSpaceFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<int>> GetWorkSpaceIdsByUserIdAsync(string userId)
        {
            return await _context.WorkSpaceFavorites
                .Where(f => f.UserId == userId)
                .Select(f => f.WorkspaceId)
                .ToListAsync();
        }
    }
}
