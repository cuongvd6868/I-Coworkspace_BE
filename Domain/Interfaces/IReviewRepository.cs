using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByRoomIdAsync(int roomId);
        Task UpdateBookingStatusAsync(int bookingId); 
        Task SaveChangesAsync();
    }
}
