using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Promotion
{
    public class PromotionResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string? DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public double MinimumAmount { get; set; }
        public bool IsActive { get; set; }
        public int? HostId { get; set; }
        // Trả về danh sách ID các Workspace áp dụng mã này
        public List<int> ApplicableWorkspaceIds { get; set; } = new();
    }
}
