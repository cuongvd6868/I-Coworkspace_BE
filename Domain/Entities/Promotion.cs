using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public string? DiscountType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UsageLimit { get; set; } = 0;
        public int UsedCount { get; set; } = 0;
        public double MinimumAmount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public int? HostId { get; set; }
        public virtual HostProfile? Host { get; set; }
    }
}
