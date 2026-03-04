using Application.DTOs.WorkSpace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IWorkSpaceService
    {
        Task<IEnumerable<WorkSpaceResponseDto>> GetAllAsync();
        Task<WorkSpaceResponseDto> GetWorkSpaceDetailsAsync(int id);
        Task<IEnumerable<WorkSpaceResponseDto>> SearchWorkSpacesAsync(string location, int? typeId);



        // Tạo mới: Validate logic trước khi lưu
        Task<WorkSpaceResponseDto> CreateWorkSpaceAsync(WorkSpaceCreateRequest dto);

        Task UpdateWorkSpaceAsync(int id, WorkSpaceUpdateRequest dto);
        Task DeleteWorkSpaceAsync(int id);
    }
}
