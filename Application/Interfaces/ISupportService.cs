using Application.DTOs.Support;

namespace Application.Interfaces
{
    public interface ISupportService
    {
        // Cho Người dùng
        Task CreateTicketAsync(CreateTicketRequest request, string userId);
        Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId);
        Task UserReplyAsync(int ticketId, string message, string userId);

        // Cho Nhân viên
        Task<IEnumerable<TicketResponseDto>> GetAllTicketsForStaffAsync();
        Task<TicketResponseDto?> GetTicketDetailsAsync(int ticketId);
        Task StaffReplyAsync(int ticketId, string message, string staffId);
        Task CloseTicketAsync(int ticketId);
    }
}