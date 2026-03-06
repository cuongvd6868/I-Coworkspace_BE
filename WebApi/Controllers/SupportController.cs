using Application.DTOs.Support;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Extensions;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/support")]
    [ApiController]
    [Authorize]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;
        private readonly ILogger<SupportController> _logger;

        public SupportController(ISupportService supportService, ILogger<SupportController> logger)
        {
            _supportService = supportService;
            _logger = logger;
        }

        // --- DÀNH CHO NGƯỜI DÙNG (GUEST/OWNER) ---

        [HttpPost("tickets")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
        {
            var userId = User.GetUserId();
            await _supportService.CreateTicketAsync(request, userId);
            return Ok(new { Message = "Yêu cầu hỗ trợ đã được gửi." });
        }

        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.GetUserId();
            var tickets = await _supportService.GetMyTicketsAsync(userId);
            return Ok(tickets);
        }

        [HttpPost("tickets/{id}/user-reply")]
        public async Task<IActionResult> UserReply(int id, [FromBody] string message)
        {
            try
            {
                var userId = User.GetUserId();
                await _supportService.UserReplyAsync(id, message, userId);
                return Ok(new { Message = "Gửi phản hồi thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- DÀNH CHO NHÂN VIÊN (STAFF/ADMIN) ---

        [HttpGet("all-tickets")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _supportService.GetAllTicketsForStaffAsync();
            return Ok(tickets);
        }

        [HttpGet("tickets/{id}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var ticket = await _supportService.GetTicketDetailsAsync(id);
            if (ticket == null) return NotFound("Không tìm thấy phiếu hỗ trợ.");
            return Ok(ticket);
        }

        [HttpPost("tickets/{id}/staff-reply")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> StaffReply(int id, [FromBody] string message)
        {
            try
            {
                var staffId = User.GetUserId();
                await _supportService.StaffReplyAsync(id, message, staffId);
                return Ok(new { Message = "Đã phản hồi khách hàng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPatch("tickets/{id}/close")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> CloseTicket(int id)
        {
            await _supportService.CloseTicketAsync(id);
            return Ok(new { Message = "Phiếu hỗ trợ đã được đóng." });
        }
    }
}