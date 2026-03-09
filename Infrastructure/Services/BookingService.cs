using Application.DTOs.Booking;
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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IBlockedTimeRepository _blockedRepo;
        private readonly IWorkSpaceRoomRepository _roomRepo;

        public BookingService(IBookingRepository bookingRepo,
                              IBlockedTimeRepository blockedRepo,
                              IWorkSpaceRoomRepository roomRepo)
        {
            _bookingRepo = bookingRepo;
            _blockedRepo = blockedRepo;
            _roomRepo = roomRepo;
        }

        // 1. Tạo mới Booking và Blocked Slot
        public async Task<BookingResponseDto> CreateBookingAsync(string userId, CreateBookingRequest request)
        {
            var isBlocked = await _blockedRepo.IsTimeOverlapAsync(request.WorkSpaceRoomId, request.StartTimeUtc, request.EndTimeUtc);
            if (isBlocked) throw new Exception("Khoảng thời gian này đã bị khóa hoặc có người đặt.");

            var room = await _roomRepo.GetByIdAsync(request.WorkSpaceRoomId);
            if (room == null) throw new Exception("Phòng không tồn tại.");

            var booking = new Booking
            {
                BookingCode = $"CSB-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                CustomerId = userId,
                WorkSpaceRoomId = request.WorkSpaceRoomId,
                StartTimeUtc = request.StartTimeUtc,
                EndTimeUtc = request.EndTimeUtc,
                TotalPrice = room.PricePerHour * (decimal)(request.EndTimeUtc - request.StartTimeUtc).TotalHours,
                BookingStatusId = 1, // Pending
                //CreatedAt = DateTime.UtcNow
            };

            var blockSlot = new BlockedTimeSlot
            {
                WorkSpaceRoomId = request.WorkSpaceRoomId,
                StartTime = request.StartTimeUtc,
                EndTime = request.EndTimeUtc,
                Reason = $"Booking {booking.BookingCode}",
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepo.AddAsync(booking);
            await _blockedRepo.AddAsync(blockSlot);
            await _bookingRepo.SaveChangesAsync();

            return booking.ToDto();
        }

        // 2. Lấy lịch sử đặt phòng của User
        public async Task<List<BookingResponseDto>> GetUserBookingHistoryAsync(string userId)
        {
            var bookings = await _bookingRepo.GetByCustomerIdAsync(userId);
            return bookings.Select(b => b.ToDto()).ToList();
        }

        // 3. Lấy chi tiết một đơn đặt phòng
        public async Task<BookingResponseDto?> GetBookingDetailsAsync(int id, string userId)
        {
            var booking = await _bookingRepo.GetByIdAsync(id);
            if (booking == null || booking.CustomerId != userId) return null;

            return booking.ToDto();
        }

        public async Task<bool> CancelBookingAsync(int id, string userId)
        {
            var booking = await _bookingRepo.GetByIdAsync(id);
            if (booking == null || booking.CustomerId != userId) return false;

            // 1. Cập nhật trạng thái Booking thành Cancelled (Giả sử ID = 3)
            booking.BookingStatusId = 3;

            // 2. Tìm Blocked Slot tương ứng để xóa
            var blockSlot = await _blockedRepo.GetByReasonAsync(booking.BookingCode);
            if (blockSlot != null)
            {
                _blockedRepo.Delete(blockSlot);
            }

            // 3. Lưu tất cả thay đổi (Atomic Transaction)
            await _bookingRepo.SaveChangesAsync();
            return true;
        }
    }
}