using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkSpaceRepository : IWorkSpaceRepository
    {
        private readonly AppDbContext _context;

        public WorkSpaceRepository(AppDbContext context)
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

        public async Task AddWorkSpaceAsync(WorkSpace workSpace)
        {
            _context.WorkSpaces.Add(workSpace);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkSpaceAsync(int id)
        {
            var workSpace = await _context.WorkSpaces.FindAsync(id);
            if (workSpace != null)
            {
                _context.WorkSpaces.Remove(workSpace);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WorkSpace>> GetAllWorkSpacesAsync()
        {
            return await _context.WorkSpaces.ToListAsync();
        }

        public async Task<IEnumerable<WorkSpace>> GetFavoriteWorkSpacesAsync(string userId)
        {
            return await _context.WorkSpaceFavorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Workspace)
                .ToListAsync();
        }

        public async Task<WorkSpace> GetWorkSpaceByIdAsync(int id)
        {
            return await _context.WorkSpaces.FindAsync(id);
        }

        public async Task<IEnumerable<WorkSpace>> GetWorkSpacesByLocationAsync(string location)
        {
            return await _context.WorkSpaces
                .Where(w => w.Address != null && 
                            (w.Address.Street.Contains(location) || 
                             w.Address.Ward.Contains(location) || 
                             w.Address.State.Contains(location) || 
                             w.Address.Country.Contains(location)))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkSpace>> GetWorkSpacesByTypeAsync(int typeId)
        {
            return await _context.WorkSpaces
                .Where(w => w.WorkSpaceTypeId == typeId)
                .ToListAsync();
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

        public async Task UpdateWorkSpaceAsync(WorkSpace workSpace)
        {
            _context.WorkSpaces.Update(workSpace);
            await _context.SaveChangesAsync();
        }
    }
}
