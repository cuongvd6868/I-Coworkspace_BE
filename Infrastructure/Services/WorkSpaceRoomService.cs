using Application.DTOs.WorkSpaceRoom;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Application.Mappings;

namespace Infrastructure.Services
{
    public class WorkSpaceRoomService : IWorkSpaceRoomService
    {
        private readonly IWorkSpaceRoomRepository _roomRepo;

        public WorkSpaceRoomService(IWorkSpaceRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<List<WorkSpaceRoomResponseDto>> GetAllAsync()
        {
            var rooms = await _roomRepo.GetAllAsync();
            return rooms.Select(r => r.ToDto()).ToList();
        }

        public async Task<WorkSpaceRoomResponseDto?> GetByIdAsync(int id)
        {
            var room = await _roomRepo.GetByIdAsync(id);
            return room?.ToDto();
        }

        public async Task<WorkSpaceRoomResponseDto> CreateAsync(CreateWorkSpaceRoomRequest request)
        {
            // Chuyển DTO sang Entity (Sử dụng Mapper đã viết ở câu trả lời trước)
            var room = request.ToEntity();

            await _roomRepo.AddAsync(room);
            await _roomRepo.SaveChangesAsync();

            // Load lại để có đầy đủ thông tin Navigations (như tên Amenity)
            var result = await _roomRepo.GetByIdAsync(room.Id);
            return result!.ToDto();
        }

        public async Task<bool> UpdateAsync(UpdateWorkSpaceRoomRequest request)
        {
            // 1. Kiểm tra phòng có tồn tại không trước khi update
            var existingRoom = await _roomRepo.GetByIdAsync(request.Id);
            if (existingRoom == null) return false;

            // 2. Tạo một entity chứa thông tin cơ bản mới từ request
            // Lưu ý: Chúng ta chỉ dùng object này để truyền dữ liệu cơ bản vào Repo
            var updatedData = new WorkSpaceRoom
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                WorkSpaceId = request.WorkSpaceId,
                WorkSpaceRoomTypeId = request.WorkSpaceRoomTypeId,
                PricePerHour = request.PricePerHour,
                Capacity = request.Capacity,
                Area = request.Area,
                IsActive = request.IsActive
            };

            // 3. Gọi hàm chuyên dụng trong Repo để xử lý "sạch" Images và Amenities
            await _roomRepo.UpdateRoomWithDetailsAsync(
                updatedData,
                request.ImageUrls ?? new List<string>(),
                request.AmenityIds ?? new List<int>()
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _roomRepo.GetByIdAsync(id);
            if (room == null) return false;

            // Kiểm tra ràng buộc kinh doanh
            if (room.Bookings != null && room.Bookings.Any(b => b.BookingStatusId != 3))
            {
                throw new Exception("Không thể xóa phòng đang có lịch đặt chỗ.");
            }

            _roomRepo.Delete(room);
            await _roomRepo.SaveChangesAsync();
            return true;
        }
    }
}