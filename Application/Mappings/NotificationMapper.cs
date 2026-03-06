using Application.DTOs.Notification;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappings
{
    public static class NotificationMapper
    {
        public static NotificationResponseDto ToDto(this Notification entity)
        {
            if (entity == null) return null!;

            return new NotificationResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreatedAt = entity.CreatedAt,
                SenderRole = entity.SenderRole,
                SenderId = entity.SenderId,
                WorkSpaceId = entity.WorkSpaceId,
                // Lấy Title từ Navigation Property nếu đã được Include trong Repository
                WorkSpaceTitle = entity.WorkSpace?.Title
            };
        }

        // Hàm hỗ trợ map danh sách thông báo
        public static IEnumerable<NotificationResponseDto> ToDtoList(this IEnumerable<Notification> entities)
        {
            return entities.Select(e => e.ToDto());
        }
    }
}