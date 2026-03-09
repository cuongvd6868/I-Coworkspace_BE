using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWorkSpaceRoomRepository
    {
        Task<WorkSpaceRoom?> GetByIdAsync(int id);
        Task<List<WorkSpaceRoom>> GetAllAsync();
        Task AddAsync(WorkSpaceRoom room);
        Task UpdateRoomWithDetailsAsync(WorkSpaceRoom updatedEntity, List<string> newImageUrls, List<int> newAmenityIds);
        void Delete(WorkSpaceRoom room);
        Task SaveChangesAsync();
    }
}