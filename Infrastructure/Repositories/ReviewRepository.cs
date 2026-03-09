using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Review review) => await _context.Reviews.AddAsync(review);

        public async Task<Review?> GetByIdAsync(int id) =>
            await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.WorkSpaceRoom)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<Review>> GetByRoomIdAsync(int roomId) =>
            await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.WorkSpaceRoomId == roomId && r.IsPublic)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

        public async Task UpdateBookingStatusAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null) booking.IsReviewed = true;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
