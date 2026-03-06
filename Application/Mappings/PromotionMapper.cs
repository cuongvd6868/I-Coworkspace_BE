using Application.DTOs.Promotion;
using Domain.Entities;

namespace Application.Mappings
{
    public static class PromotionMapper
    {
        public static PromotionResponseDto ToDto(this Promotion entity)
        {
            if (entity == null) return null!;
            return new PromotionResponseDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                DiscountValue = entity.DiscountValue,
                DiscountType = entity.DiscountType,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                UsageLimit = entity.UsageLimit,
                UsedCount = entity.UsedCount,
                MinimumAmount = entity.MinimumAmount,
                IsActive = entity.IsActive,
                HostId = entity.HostId,
                ApplicableWorkspaceIds = entity.WorkSpacePromotions.Select(wp => wp.WorkSpaceId).ToList()
            };
        }
    }
}