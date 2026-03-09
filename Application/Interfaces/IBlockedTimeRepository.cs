using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces // Đưa về đúng Namespace Domain
{
    public interface IBlockedTimeRepository
    {
        Task AddAsync(BlockedTimeSlot slot);

        // Dùng để kiểm tra trùng lịch trước khi cho phép Booking
        Task<bool> IsTimeOverlapAsync(int roomId, DateTime start, DateTime end);

        // Dùng để tìm kiếm Slot cần xóa khi hủy Booking (Tìm theo BookingCode)
        Task<BlockedTimeSlot?> GetByReasonAsync(string reason);

        // Xóa slot để giải phóng lịch phòng
        void Delete(BlockedTimeSlot slot);

        Task SaveChangesAsync();
    }
}