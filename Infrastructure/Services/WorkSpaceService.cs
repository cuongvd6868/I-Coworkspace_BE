using Application.DTOs.WorkSpace;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WorkSpaceService : IWorkSpaceService
    {
        private readonly IWorkSpaceRepository _workSpaceRepo;

        public WorkSpaceService(IWorkSpaceRepository workSpaceRepo)
        {
            _workSpaceRepo = workSpaceRepo;
        }

        public async Task<List<WorkSpaceResponseDto>> GetAllAsync()
        {
            var workspaces = await _workSpaceRepo.GetAllWorkSpacesAsync();
            return workspaces.Select(w => w.ToDto()).ToList();
        }

        public async Task<WorkSpaceResponseDto?> GetByIdAsync(int id)
        {
            var workspace = await _workSpaceRepo.GetWorkSpaceByIdAsync(id);
            return workspace?.ToDto();
        }

        public async Task<WorkSpaceResponseDto> CreateAsync(WorkSpaceCreateRequest request)
        {
            var workspaceEntity = request.ToEntity();

            await _workSpaceRepo.AddWorkSpaceAsync(workspaceEntity);

            var result = await _workSpaceRepo.GetWorkSpaceByIdAsync(workspaceEntity.Id);
            return result!.ToDto();
        }

        public async Task<bool> UpdateAsync(WorkSpaceUpdateRequest request)
        {
            var existing = await _workSpaceRepo.GetWorkSpaceByIdAsync(request.Id);
            if (existing == null) return false;

            var workspaceToUpdate = new WorkSpace
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                WorkSpaceTypeId = request.WorkSpaceTypeId,
                AddressId = request.AddressId,
                IsActive = request.IsActive,
                HostId = existing.HostId 
            };

            await _workSpaceRepo.UpdateWorkSpaceAsync(workspaceToUpdate, request.ImageUrls ?? new List<string>());

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _workSpaceRepo.GetWorkSpaceByIdAsync(id);
            if (existing == null) return false;

            if (existing.WorkSpaceRooms.Any(r => r.Bookings.Any(b => b.BookingStatusId != 3))) // 3 là Cancelled
            {
                throw new Exception("Không thể xóa Workspace khi có phòng đang có lịch đặt.");
            }

            await _workSpaceRepo.DeleteWorkSpaceAsync(id);
            return true;
        }

        // 6. Tìm kiếm theo vị trí
        public async Task<List<WorkSpaceResponseDto>> GetByLocationAsync(string location)
        {
            var result = await _workSpaceRepo.GetWorkSpacesByLocationAsync(location);
            return result.Select(w => w.ToDto()).ToList();
        }

        // 7. Lọc theo loại hình Workspace
        public async Task<List<WorkSpaceResponseDto>> GetByTypeAsync(int typeId)
        {
            var result = await _workSpaceRepo.GetWorkSpacesByTypeAsync(typeId);
            return result.Select(w => w.ToDto()).ToList();
        }
    }
}