using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) => _context = context;

        public async Task<Booking?> GetByIdAsync(int id) =>
            await _context.Bookings
                .Include(b => b.WorkSpaceRoom)
                .Include(b => b.BookingStatus)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<List<Booking>> GetByCustomerIdAsync(string customerId) =>
            await _context.Bookings
                .Include(b => b.WorkSpaceRoom)
                .Include(b => b.BookingStatus)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.StartTimeUtc)
                .ToListAsync();

        public async Task AddAsync(Booking booking) => await _context.Bookings.AddAsync(booking);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public Task<bool> IsRoomAvailableAsync(int roomId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}