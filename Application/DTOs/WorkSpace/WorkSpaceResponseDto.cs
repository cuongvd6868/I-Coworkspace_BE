using Application.DTOs.WorkSpaceRoom;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.WorkSpace
{
    public class WorkSpaceResponseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;
        // host
        public int HostId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPhone { get; set; }
        public string? Avatar { get; set; }
        //type
        public string? WorkSpaceTypeName { get; set; }
        //image
        public List<string> ImageUrls { get; set; } = new();
        //rooms
        // Trong file Application/DTOs/WorkSpace/WorkSpaceResponseDto.cs
        public List<WorkSpaceRoomResponseDto> WorkSpaceRooms { get; set; } = new();
    }
}
