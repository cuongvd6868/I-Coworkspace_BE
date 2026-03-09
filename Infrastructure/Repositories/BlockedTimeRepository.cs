using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BlockedTimeRepository : IBlockedTimeRepository
    {
        private readonly AppDbContext _context;
        public BlockedTimeRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(BlockedTimeSlot slot) => await _context.BlockedTimeSlots.AddAsync(slot);

        public async Task<bool> IsTimeOverlapAsync(int roomId, DateTime start, DateTime end)
        {
            return await _context.BlockedTimeSlots.AnyAsync(s =>
                s.WorkSpaceRoomId == roomId &&
                ((start >= s.StartTime && start < s.EndTime) ||
                 (end > s.StartTime && end <= s.EndTime) ||
                 (start <= s.StartTime && end >= s.EndTime)));
        }

        public async Task<BlockedTimeSlot?> GetByReasonAsync(string reason)
        {
            // Tìm slot bị khóa dựa trên chuỗi "Booking [BookingCode]"
            return await _context.BlockedTimeSlots
                .FirstOrDefaultAsync(s => s.Reason != null && s.Reason.Contains(reason));
        }

        public void Delete(BlockedTimeSlot slot)
        {
            _context.BlockedTimeSlots.Remove(slot);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}