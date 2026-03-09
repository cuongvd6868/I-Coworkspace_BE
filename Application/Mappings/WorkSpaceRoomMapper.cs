using Application.DTOs.WorkSpaceRoom;
using Domain.Entities;

namespace Application.Mappings
{
    public static class WorkSpaceRoomMapper
    {
        public static WorkSpaceRoomResponseDto ToDto(this WorkSpaceRoom entity)
        {
            if (entity == null) return null!;

            return new WorkSpaceRoomResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                PricePerHour = entity.PricePerHour,
                Capacity = entity.Capacity,
                Area = entity.Area,
                IsActive = entity.IsActive,

                // Map danh sách ảnh
                Images = entity.WorkSpaceRoomImages?.Select(img => new WorkSpaceRoomImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    Caption = img.Caption
                }).ToList() ?? new(),

                // Map danh sách tên tiện ích (lấy từ bảng Amenity thông qua bảng trung gian)
                Amenities = entity.WorkSpaceRoomAmenities?
                    .Where(ra => ra.Amenity != null)
                    .Select(ra => ra.Amenity!.Name)
                    .ToList() ?? new()
            };
        }

        public static WorkSpaceRoom ToEntity(this CreateWorkSpaceRoomRequest dto)
        {
            var entity = new WorkSpaceRoom
            {
                Title = dto.Title,
                Description = dto.Description,
                WorkSpaceId = dto.WorkSpaceId,
                WorkSpaceRoomTypeId = dto.WorkSpaceRoomTypeId,
                PricePerHour = dto.PricePerHour,
                Capacity = dto.Capacity,
                Area = dto.Area,
                IsActive = true,
                // Map Images
                WorkSpaceRoomImages = dto.ImageUrls?.Select(url => new WorkSpaceRoomImage
                {
                    ImageUrl = url
                }).ToList() ?? new(),
                // Map Amenities
                WorkSpaceRoomAmenities = dto.AmenityIds?.Select(id => new WorkSpaceRoomAmenity
                {
                    AmenityId = id
                }).ToList() ?? new()
            };
            return entity;
        }
    }
}