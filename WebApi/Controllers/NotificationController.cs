using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Extensions;
using Application.DTOs.Notification;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;

namespace WebApi.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập để sử dụng các tính năng thông báo
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách thông báo của người dùng hiện tại (Global + Workspace yêu thích)
        /// </summary>
        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            try
            {
                var userId = User.GetUserId(); // Lấy UserId từ Token
                var notifications = await _notificationService.GetMyNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thông báo");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Tạo thông báo mới (Admin gửi toàn hệ thống, Owner gửi cho Workspace của mình)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notificationRequest)
        {
            try
            {
                var userId = User.GetUserId();
                // Lấy Role từ Claims để truyền vào Service xử lý logic phân quyền
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "";

                await _notificationService.CreateNotificationAsync(notificationRequest, userId, role);

                return Ok(new { Message = "Thông báo đã được gửi thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông báo");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xóa thông báo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "";

                await _notificationService.DeleteNotificationAsync(id, userId, role);
                return Ok(new { Message = "Đã xóa thông báo." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông báo");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}