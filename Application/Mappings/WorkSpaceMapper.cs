using Application.DTOs.WorkSpace;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

                // Map Host Information
                HostId = entity.HostId,
                CompanyName = entity.Host?.CompanyName,
                ContactPhone = entity.Host?.ContactPhone,
                Avatar = entity.Host?.Avatar,

                // Map Type
                WorkSpaceTypeName = entity.WorkSpaceType?.Name,

                // Map Images (Chuyển danh sách object sang danh sách string url)
                ImageUrls = entity.WorkSpaceImages?
                            .Select(img => img.ImageUrl)
                            .ToList() ?? new List<string>(),
                //WorkSpaceRooms = entity.WorkSpaceRooms?
                //            .Select(room => new WorkSpaceRoom
                //            {
                //                Id = room.Id,
                //                //Title = room.Title,
                //                //Description = room.Description,
                //                Capacity = room.Capacity,
                //                PricePerHour = room.PricePerHour
                //            }).ToList() ?? new List<string>()

            };
        }
    }
}
