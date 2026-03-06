using Application.DTOs.Promotion;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepo;
        private readonly IHostProfileRepository _hostRepo;
        private readonly IWorkSpaceRepository _workspaceRepo;

        public PromotionService(IPromotionRepository promotionRepo, IHostProfileRepository hostRepo, IWorkSpaceRepository workspaceRepo)
        {
            _promotionRepo = promotionRepo;
            _hostRepo = hostRepo;
            _workspaceRepo = workspaceRepo;
        }

        public async Task<IEnumerable<PromotionResponseDto>> GetAllPromotionsAsync()
        {
            var entities = await _promotionRepo.GetAllAsync();
            return entities.Select(e => e.ToDto());
        }

        public async Task<PromotionResponseDto?> GetPromotionByCodeAsync(string code)
        {
            var entity = await _promotionRepo.GetByCodeAsync(code);
            return entity?.ToDto();
        }

        public async Task CreatePromotionAsync(CreatePromotionRequest request, string userId, string role)
        {
            var promotion = new Promotion
            {
                Code = request.Code.ToUpper(),
                Description = request.Description,
                DiscountValue = request.DiscountValue,
                DiscountType = request.DiscountType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UsageLimit = request.UsageLimit,
                MinimumAmount = request.MinimumAmount,
                IsActive = true
            };

            if (role == "Admin")
            {
                promotion.HostId = null; // Mã toàn hệ thống
            }
            else if (role == "Owner")
            {
                var host = await _hostRepo.GetHostProfileByUserIdAsync(userId);
                if (host == null) throw new UnauthorizedAccessException("Không tìm thấy hồ sơ Host.");

                promotion.HostId = host.Id;

                // Nếu Owner chọn danh sách Workspace cụ thể
                if (request.WorkspaceIds != null && request.WorkspaceIds.Any())
                {
                    foreach (var wsId in request.WorkspaceIds)
                    {
                        var isOwned = await _workspaceRepo.IsOwnerOfWorkspaceAsync(host.Id, wsId);
                        if (!isOwned) throw new UnauthorizedAccessException($"Bạn không sở hữu Workspace ID: {wsId}");

                        promotion.WorkSpacePromotions.Add(new WorkSpacePromotion { WorkSpaceId = wsId });
                    }
                }
            }

            await _promotionRepo.AddAsync(promotion);
        }

        public async Task<PromotionResponseDto?> ValidatePromotionForBookingAsync(string code, int workspaceId, double bookingAmount)
        {
            var promotion = await _promotionRepo.GetValidPromotionByWorkspaceAsync(code, workspaceId);

            if (promotion == null) return null;

            // Kiểm tra số tiền tối thiểu
            if (bookingAmount < promotion.MinimumAmount) return null;

            return promotion.ToDto();
        }

        public async Task UpdatePromotionStatusAsync(int id, bool isActive)
        {
            var promotion = await _promotionRepo.GetByIdAsync(id);
            if (promotion != null)
            {
                promotion.IsActive = isActive;
                await _promotionRepo.UpdateAsync(promotion);
            }
        }

        public async Task DeletePromotionAsync(int id, string userId, string role)
        {
            var promotion = await _promotionRepo.GetByIdAsync(id);
            if (promotion == null) return;

            if (role == "Admin")
            {
                await _promotionRepo.DeleteAsync(id);
            }
            else
            {
                var host = await _hostRepo.GetHostProfileByUserIdAsync(userId);
                if (promotion.HostId == host?.Id)
                {
                    await _promotionRepo.DeleteAsync(id);
                }
                else throw new UnauthorizedAccessException("Bạn không có quyền xóa mã này.");
            }
        }
    }
}