using Application.DTOs.Promotion;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Extensions;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<PromotionController> _logger;

        public PromotionController(IPromotionService promotionService, ILogger<PromotionController> logger)
        {
            _promotionService = promotionService;
            _logger = logger;
        }


        [HttpGet("check-valid")]
        public async Task<IActionResult> ValidatePromotion(string code, int workspaceId, double amount)
        {
            var result = await _promotionService.ValidatePromotionForBookingAsync(code, workspaceId, amount);
            if (result == null)
            {
                return BadRequest(new { Message = "Mã giảm giá không hợp lệ, đã hết hạn hoặc không áp dụng cho không gian này." });
            }
            return Ok(result);
        }

        // --- ADMIN & OWNER API ---

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetAll()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
        {
            try
            {
                var userId = User.GetUserId();
                var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

                await _promotionService.CreatePromotionAsync(request, userId, role);
                return Ok(new { Message = "Tạo mã khuyến mãi thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo Promotion");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isActive)
        {
            await _promotionService.UpdatePromotionStatusAsync(id, isActive);
            return Ok(new { Message = "Cập nhật trạng thái thành công." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.GetUserId();
                var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

                await _promotionService.DeletePromotionAsync(id, userId, role);
                return Ok(new { Message = "Xóa mã khuyến mãi thành công." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}