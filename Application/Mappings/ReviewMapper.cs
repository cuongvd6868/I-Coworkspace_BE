using Application.DTOs.Review;
using Domain.Entities;

namespace Application.Mappings
{
    public static class ReviewMapper
    {
        public static ReviewResponseDto ToDto(this Review entity)
        {
            if (entity == null) return null!;
            return new ReviewResponseDto
            {
                Id = entity.Id,
                BookingId = entity.BookingId,
                Rating = entity.Rating,
                Comment = entity.Comment,
                UserName = entity.User?.UserName ?? "Nguời dùng",
                RoomName = entity.WorkSpaceRoom?.Title ?? "Phòng không xác định",
                IsVerified = entity.IsVerified
            };
        }

        public static Review ToEntity(this CreateReviewRequest dto, string userId, int roomId)
        {
            if (dto == null) return null!;
            return new Review
            {
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = userId,
                WorkSpaceRoomId = roomId,
                IsVerified = true,
                IsPublic = true
            };
        }
    }
}