using Application.DTOs.WorkSpaceRoom;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.WorkSpace
{
    public class WorkSpaceUpdateRequest
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int WorkSpaceTypeId { get; set; }
        public int AddressId { get; set; }
        public bool IsActive { get; set; }

        public List<string>? ImageUrls { get; set; } = new();
    }
}
