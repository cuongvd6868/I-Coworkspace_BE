using Application.DTOs.Booking;
using Application.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService) => _bookingService = bookingService;

        private string CurrentUserId => User.GetUserId() ?? "uid"; 

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            try
            {
                var result = await _bookingService.CreateBookingAsync(CurrentUserId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("my-history")]
        public async Task<IActionResult> GetMyHistory()
            => Ok(await _bookingService.GetUserBookingHistoryAsync(CurrentUserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var result = await _bookingService.GetBookingDetailsAsync(id, CurrentUserId);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _bookingService.CancelBookingAsync(id, CurrentUserId);
            return result ? Ok(new { Message = "Đã hủy đơn đặt phòng và mở lại lịch trống." })
                          : BadRequest("Không thể hủy đơn này.");
        }
    }
}