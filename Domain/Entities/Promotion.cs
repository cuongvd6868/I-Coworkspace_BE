using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public decimal DiscountValue { get; set; }
        public string? DiscountType { get; set; } // "Percentage" hoặc "FixedAmount"

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UsageLimit { get; set; } = 0;
        public int UsedCount { get; set; } = 0;
        public double MinimumAmount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Nếu null = Admin tạo (Toàn hệ thống)
        public int? HostId { get; set; }
        public virtual HostProfile? Host { get; set; } 

        // Quan hệ N-N: Một mã khuyến mãi có thể áp dụng cho nhiều Workspace (của cùng 1 chủ)
        // Hoặc đơn giản hơn là 1-N nếu bạn muốn mỗi mã chỉ áp dụng cho 1 Workspace cụ thể
        public virtual List<WorkSpacePromotion> WorkSpacePromotions { get; set; } = new();
    }

    // Bảng trung gian để định nghĩa mã này áp dụng cho Workspace nào

}