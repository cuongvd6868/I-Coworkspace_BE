using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetByCustomerIdAsync(string customerId);
        Task AddAsync(Booking booking);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime start, DateTime end);
        Task SaveChangesAsync();
    }
}