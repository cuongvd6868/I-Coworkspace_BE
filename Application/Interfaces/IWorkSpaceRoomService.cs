using Application.DTOs.WorkSpaceRoom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkSpaceRoomService
    {
        Task<WorkSpaceRoomResponseDto> CreateAsync(CreateWorkSpaceRoomRequest request);
        Task<List<WorkSpaceRoomResponseDto>> GetAllAsync();
        Task<WorkSpaceRoomResponseDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(UpdateWorkSpaceRoomRequest request);
        Task<bool> DeleteAsync(int id);
    }
}