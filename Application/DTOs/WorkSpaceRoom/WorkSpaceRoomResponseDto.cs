using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.WorkSpaceRoom
{
    public class WorkSpaceRoomResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public double Area { get; set; }
        public bool IsActive { get; set; }

        // Hiển thị thêm danh sách ảnh
        public List<WorkSpaceRoomImageDto> Images { get; set; } = new();

        // Hiển thị thêm danh sách tên tiện ích
        public List<string> Amenities { get; set; } = new();
    }

    public class WorkSpaceRoomImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }
    }
}