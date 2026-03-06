using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.WorkSpace)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        // Logic quan trọng: Lọc thông báo dựa trên danh sách Workspace người dùng quan tâm
        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(List<int> favoriteWorkspaceIds)
        {
            return await _context.Notifications
                .Where(n => n.WorkSpaceId == null || favoriteWorkspaceIds.Contains(n.WorkSpaceId.Value))
                .OrderByDescending(n => n.CreatedAt) // Sắp xếp mới nhất lên đầu
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByWorkSpaceIdAsync(int workSpaceId)
        {
            return await _context.Notifications
                .Where(n => n.WorkSpaceId == workSpaceId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetSystemNotificationsAsync()
        {
            return await _context.Notifications
                .Where(n => n.WorkSpaceId == null && n.SenderRole == "Admin")
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}