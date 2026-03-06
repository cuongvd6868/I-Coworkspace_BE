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
            return await _context.WorkSpaces
                .Include(w => w.Address)
                .Include(w => w.WorkSpaceType)
                .Include(w => w.WorkSpaceRooms)
                    .ThenInclude(r => r.WorkSpaceRoomAmenities)
                        .ThenInclude(ra => ra.Amenity) // Lấy tên tiện ích (Wifi, Máy chiếu...)
                .Where(w => w.IsActive)
                .ToListAsync();
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
            return await _context.WorkSpaces
                .Include(w => w.Address)
                .Include(w => w.Host)
                .Include(w => w.WorkSpaceType)
                .Include(w => w.WorkSpaceRooms)
                .Include(w => w.WorkSpaceImages)
                .FirstOrDefaultAsync(w => w.Id == id);
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
                .Include(w => w.Address)
                .Include(w => w.Host)
                .Include(w => w.WorkSpaceType)
                .Include(w => w.WorkSpaceImages)
                .Where(w => w.WorkSpaceTypeId == typeId)
                .ToListAsync();
        }

        public async Task UpdateWorkSpaceAsync(WorkSpace workSpace)
        {
            _context.WorkSpaces.Update(workSpace);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsOwnerOfWorkspaceAsync(int hostId, int workspaceId)
        {
            return await _context.WorkSpaces
                .AnyAsync(w => w.Id == workspaceId && w.HostId == hostId);
        }
    }
}
