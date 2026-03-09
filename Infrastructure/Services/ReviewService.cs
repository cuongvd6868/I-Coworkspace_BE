using Application.DTOs.Review;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IBookingRepository _bookingRepo; 


        public ReviewService(IReviewRepository reviewRepo, IBookingRepository bookingRepo)
        {
            _reviewRepo = reviewRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(string userId, CreateReviewRequest request)
        {
            // 1. Kiểm tra Booking (Dùng repo Booking của bạn)
            var booking = await _bookingRepo.GetByIdAsync(request.BookingId);

            if (booking == null || booking.CustomerId != userId)
                throw new Exception("Đơn đặt phòng không hợp lệ hoặc không thuộc về bạn.");

            if (booking.IsReviewed)
                throw new Exception("Đơn đặt phòng này đã được đánh giá.");

            // 2. Tạo Entity Review
            var review = new Review
            {
                BookingId = request.BookingId,
                UserId = userId,
                WorkSpaceRoomId = booking.WorkSpaceRoomId,
                Rating = request.Rating,
                Comment = request.Comment,
                IsVerified = booking.CheckedOutAt != null,
                IsPublic = true
            };

            // 3. Thực thi lưu dữ liệu
            await _reviewRepo.AddAsync(review);
            await _reviewRepo.UpdateBookingStatusAsync(booking.Id); // Đánh dấu IsReviewed = true
            await _reviewRepo.SaveChangesAsync();

            return review.ToDto();
        }
    }
}
