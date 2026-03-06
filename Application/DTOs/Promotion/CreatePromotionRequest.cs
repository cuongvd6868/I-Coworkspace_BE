using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Promotion
{
    public class CreatePromotionRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } = "Percentage";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public double MinimumAmount { get; set; }
        // Danh sách Workspace muốn áp dụng (Chỉ dành cho Owner)
        public List<int>? WorkspaceIds { get; set; }
    }
}
