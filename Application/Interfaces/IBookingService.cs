using Application.DTOs.Booking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(string userId, CreateBookingRequest request);
        Task<List<BookingResponseDto>> GetUserBookingHistoryAsync(string userId);
        Task<BookingResponseDto?> GetBookingDetailsAsync(int id, string userId);
        Task<bool> CancelBookingAsync(int id, string userId); 
    }
}