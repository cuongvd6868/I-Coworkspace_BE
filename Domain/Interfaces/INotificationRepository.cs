using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(int id);

        // Lấy thông báo cho người dùng: Thông báo Admin (null) + Thông báo từ Workspace đã thích
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(List<int> favoriteWorkspaceIds);

        // Lấy tất cả thông báo của một Workspace cụ thể (cho Owner quản lý)
        Task<IEnumerable<Notification>> GetByWorkSpaceIdAsync(int workSpaceId);

        // Lấy tất cả thông báo hệ thống của Admin
        Task<IEnumerable<Notification>> GetSystemNotificationsAsync();

        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
    }
}