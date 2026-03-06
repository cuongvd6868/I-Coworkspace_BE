using Application.DTOs.Notification;
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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IWorkSpaceRepository _workSpaceRepository;
        private readonly IHostProfileRepository _hostProfileRepository;
        // Giả sử bạn có Repo này để lấy danh sách yêu thích
        private readonly IWorkSpaceFavoriteRepository _favoriteRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IWorkSpaceRepository workSpaceRepository,
            IHostProfileRepository hostProfileRepository,
            IWorkSpaceFavoriteRepository favoriteRepository)
        {
            _notificationRepository = notificationRepository;
            _workSpaceRepository = workSpaceRepository;
            _hostProfileRepository = hostProfileRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId)
        {
            // 1. Lấy danh sách ID các Workspace mà user đã Favorite
            var favoriteIds = await _favoriteRepository.GetWorkSpaceIdsByUserIdAsync(userId);

            // 2. Gọi Repo để lấy thông báo Global (null) + Thông báo từ Workspace đã thích
            var notifications = await _notificationRepository.GetNotificationsForUserAsync(favoriteIds.ToList());

            // 3. Map sang DTO
            return notifications.Select(n => n.ToDto());
        }

        public async Task CreateNotificationAsync(Notification notification, string userId, string role)
        {
            if (role == "Admin")
            {
                // Admin mặc định gửi toàn hệ thống (WorkSpaceId có thể null)
                notification.SenderRole = "Admin";
                await _notificationRepository.AddAsync(notification);
            }
            else if (role == "Owner")
            {
                // Logic: Kiểm tra Owner có thực sự sở hữu Workspace này không
                var host = await _hostProfileRepository.GetHostProfileByUserIdAsync(userId);
                if (host == null) throw new UnauthorizedAccessException("Không tìm thấy hồ sơ Host.");

                if (!notification.WorkSpaceId.HasValue)
                    throw new ArgumentException("Owner phải chọn một Workspace để gửi thông báo.");

                var isOwned = await _workSpaceRepository.IsOwnerOfWorkspaceAsync(host.Id, notification.WorkSpaceId.Value);
                if (!isOwned)
                    throw new UnauthorizedAccessException("Bạn không có quyền gửi thông báo cho Workspace này.");

                notification.SenderRole = "Owner";
                notification.SenderId = host.Id;
                await _notificationRepository.AddAsync(notification);
            }
        }

        public async Task DeleteNotificationAsync(int id, string userId, string role)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return;

            // Admin xóa bất cứ cái gì, Owner chỉ xóa được thông báo của mình
            if (role == "Admin")
            {
                await _notificationRepository.DeleteAsync(id);
            }
            else if (role == "Owner")
            {
                var host = await _hostProfileRepository.GetHostProfileByUserIdAsync(userId);
                if (notification.SenderId == host?.Id)
                {
                    await _notificationRepository.DeleteAsync(id);
                }
                else
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền xóa thông báo này.");
                }
            }
        }
    }
}