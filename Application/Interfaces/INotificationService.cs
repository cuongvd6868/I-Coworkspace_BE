using Application.DTOs.Notification;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        // Lấy thông báo cho User (gồm Global + Workspace yêu thích)
        Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId);

        // Tạo thông báo (Dùng chung cho Admin và Owner)
        Task CreateNotificationAsync(Notification notification, string userId, string role);

        // Xóa thông báo
        Task DeleteNotificationAsync(int id, string userId, string role);
    }
}