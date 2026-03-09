using Application.DTOs.WorkSpace;
using Application.DTOs.WorkSpaceRoom;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Mappings;

namespace Application.Mappings
{
    public static class WorkSpaceMapper
    {
        public static WorkSpaceResponseDto ToDto(this WorkSpace entity)
        {
            if (entity == null) return null!;

            return new WorkSpaceResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsActive = entity.IsActive,
                IsVerified = entity.IsVerified,

                HostId = entity.HostId,
                CompanyName = entity.Host?.CompanyName,
                ContactPhone = entity.Host?.ContactPhone,
                Avatar = entity.Host?.Avatar,

                WorkSpaceTypeName = entity.WorkSpaceType?.Name,

                ImageUrls = entity.WorkSpaceImages?
                            .Select(img => img.ImageUrl)
                            .ToList() ?? new List<string>(),

                WorkSpaceRooms = entity.WorkSpaceRooms?
                            .Select(room => room.ToDto())
                            .ToList() ?? new List<WorkSpaceRoomResponseDto>()
            };
        }

        public static WorkSpace ToEntity(this WorkSpaceCreateRequest dto)
        {
            var workspace = new WorkSpace
            {
                Title = dto.Title,
                Description = dto.Description,
                HostId = dto.HostId,
                AddressId = dto.AddressId,
                WorkSpaceTypeId = dto.WorkSpaceTypeId,
                IsActive = true,
                // Map ảnh Workspace
                WorkSpaceImages = dto.ImageUrls?.Select(url => new WorkSpaceImage { ImageUrl = url }).ToList() ?? new(),

                // Map danh sách phòng (Deep Insert)
                WorkSpaceRooms = dto.Rooms?.Select(r => new WorkSpaceRoom
                {
                    Title = r.Title,
                    Description = r.Description,
                    WorkSpaceRoomTypeId = r.WorkSpaceRoomTypeId,
                    PricePerHour = r.PricePerHour,
                    Capacity = r.Capacity,
                    Area = r.Area,
                    IsActive = true,
                    // Nếu trong CreateWorkSpaceRoomRequest có ImageUrls/AmenityIds, map tiếp tại đây
                    WorkSpaceRoomImages = r.ImageUrls?.Select(url => new WorkSpaceRoomImage { ImageUrl = url }).ToList() ?? new(),
                    WorkSpaceRoomAmenities = r.AmenityIds?.Select(id => new WorkSpaceRoomAmenity { AmenityId = id }).ToList() ?? new()
                }).ToList() ?? new()
            };
            return workspace;
        }
    }
}
