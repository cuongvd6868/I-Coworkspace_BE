using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WorkSpaceRoomRepository : IWorkSpaceRoomRepository
    {
        private readonly AppDbContext _context;

        public WorkSpaceRoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkSpaceRoom?> GetByIdAsync(int id)
        {
            return await _context.WorkSpaceRooms
                .Include(r => r.WorkSpace)
                .Include(r => r.WorkSpaceRoomType)
                .Include(r => r.WorkSpaceRoomImages) 
                .Include(r => r.WorkSpaceRoomAmenities).ThenInclude(ra => ra.Amenity) 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<WorkSpaceRoom>> GetAllAsync()
        {
            return await _context.WorkSpaceRooms
                .Include(r => r.WorkSpaceRoomImages)
                .AsNoTracking() 
                .ToListAsync();
        }

        public async Task AddAsync(WorkSpaceRoom room)
        {
            await _context.WorkSpaceRooms.AddAsync(room);
        }

        public async Task UpdateRoomWithDetailsAsync(WorkSpaceRoom updatedEntity, List<string> newImageUrls, List<int> newAmenityIds)
        {
            var existingRoom = await _context.WorkSpaceRooms
                .Include(r => r.WorkSpaceRoomImages)
                .Include(r => r.WorkSpaceRoomAmenities)
                .FirstOrDefaultAsync(r => r.Id == updatedEntity.Id);

            if (existingRoom == null) return;

            _context.Entry(existingRoom).CurrentValues.SetValues(updatedEntity);

            _context.WorkSpaceRoomImages.RemoveRange(existingRoom.WorkSpaceRoomImages);
            existingRoom.WorkSpaceRoomImages = newImageUrls.Select(url => new WorkSpaceRoomImage
            {
                ImageUrl = url,
                WorkSpaceRoomId = existingRoom.Id
            }).ToList();

            _context.WorkSpaceRoomAmenities.RemoveRange(existingRoom.WorkSpaceRoomAmenities);
            existingRoom.WorkSpaceRoomAmenities = newAmenityIds.Select(id => new WorkSpaceRoomAmenity
            {
                AmenityId = id,
                WorkSpaceRoomId = existingRoom.Id
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public void Delete(WorkSpaceRoom room)
        {
            _context.WorkSpaceRooms.Remove(room);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}