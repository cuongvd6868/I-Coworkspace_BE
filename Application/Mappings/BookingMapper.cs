using Application.DTOs.Booking;
using Domain.Entities;

namespace Application.Mappings
{
    public static class BookingMapper
    {
        public static BookingResponseDto ToDto(this Booking entity)
        {
            if (entity == null) return null!;
            return new BookingResponseDto
            {
                Id = entity.Id,
                BookingCode = entity.BookingCode,
                RoomName = entity.WorkSpaceRoom?.Title,
                StartTimeUtc = entity.StartTimeUtc,
                EndTimeUtc = entity.EndTimeUtc,
                FinalAmount = entity.FinalAmount,
                StatusName = entity.BookingStatus?.Name,
                IsReviewed = entity.IsReviewed
            };
        }
    }
}