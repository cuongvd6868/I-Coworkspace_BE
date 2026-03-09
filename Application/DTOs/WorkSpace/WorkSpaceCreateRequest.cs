using Application.DTOs.WorkSpaceRoom; 

namespace Application.DTOs.WorkSpace
{
    public class WorkSpaceCreateRequest
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int HostId { get; set; }
        public int AddressId { get; set; }
        public int WorkSpaceTypeId { get; set; }
        public List<string>? ImageUrls { get; set; }

        public List<CreateWorkSpaceRoomRequest>? Rooms { get; set; } = new();
    }
}