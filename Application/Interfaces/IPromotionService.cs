using Application.DTOs.Promotion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionResponseDto>> GetAllPromotionsAsync();
        Task<PromotionResponseDto?> GetPromotionByCodeAsync(string code);
        // Tạo mã mới kèm phân quyền Role
        Task CreatePromotionAsync(CreatePromotionRequest request, string userId, string role);
        Task UpdatePromotionStatusAsync(int id, bool isActive);
        Task DeletePromotionAsync(int id, string userId, string role);
        // Kiểm tra mã giảm giá khi đặt phòng
        Task<PromotionResponseDto?> ValidatePromotionForBookingAsync(string code, int workspaceId, double bookingAmount);
    }
}