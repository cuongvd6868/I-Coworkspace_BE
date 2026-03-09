using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.WorkSpaceRoom
{
    public class CreateWorkSpaceRoomRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int WorkSpaceId { get; set; }
        public int WorkSpaceRoomTypeId { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public double Area { get; set; }

        // Thêm danh sách ảnh và tiện ích
        public List<string>? ImageUrls { get; set; }
        public List<int>? AmenityIds { get; set; }
    }
}
