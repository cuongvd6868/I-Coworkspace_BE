using Application.DTOs.WorkSpace;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkSpaceService
    {
        Task<WorkSpaceResponseDto> CreateAsync(WorkSpaceCreateRequest request);
        Task<List<WorkSpaceResponseDto>> GetAllAsync();
        Task<WorkSpaceResponseDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(WorkSpaceUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<WorkSpaceResponseDto>> GetByLocationAsync(string location);
        Task<List<WorkSpaceResponseDto>> GetByTypeAsync(int typeId);
    }
}