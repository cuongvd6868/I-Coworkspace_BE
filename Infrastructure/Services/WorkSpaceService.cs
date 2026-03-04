using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.WorkSpace;
using Application.Interfaces;
using Application.Mappings;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class WorkSpaceService : IWorkSpaceService
    {
        private readonly IWorkSpaceRepository _workSpaceRepository;

        public WorkSpaceService(IWorkSpaceRepository workSpaceRepository)
        {
            _workSpaceRepository = workSpaceRepository;
        }

        public async Task<IEnumerable<WorkSpaceResponseDto>> GetAllAsync()
        {
            var list = await _workSpaceRepository.GetAllWorkSpacesAsync();
            return list.Select(w => w.ToDto());
        }

        public async Task<WorkSpaceResponseDto> GetWorkSpaceDetailsAsync(int id)
        {
            var workSpace = await _workSpaceRepository.GetWorkSpaceByIdAsync(id);
            return workSpace.ToDto();
        }

        public Task<IEnumerable<WorkSpaceResponseDto>> SearchWorkSpacesAsync(string location, int? typeId)
        {
            throw new NotImplementedException();
        }

        public Task<WorkSpaceResponseDto> CreateWorkSpaceAsync(WorkSpaceCreateRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWorkSpaceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWorkSpaceAsync(int id, WorkSpaceUpdateRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}
