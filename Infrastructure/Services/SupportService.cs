using Application.DTOs.Support;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepo;

        public SupportService(ISupportRepository supportRepo)
        {
            _supportRepo = supportRepo;
        }

        public async Task CreateTicketAsync(CreateTicketRequest request, string userId)
        {
            var ticket = new SupportTicket
            {
                Subject = request.Subject,
                Message = request.Message,
                TicketType = (SupportTicketType)request.TicketType,
                Status = SupportTicketStatus.New,
                SubmittedByUserId = userId
            };
            await _supportRepo.AddTicketAsync(ticket);
        }

        public async Task StaffReplyAsync(int ticketId, string message, string staffId)
        {
            var ticket = await _supportRepo.GetTicketByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket không tồn tại.");

            var reply = new SupportTicketReply
            {
                TicketId = ticketId,
                Message = message,
                RepliedByUserId = staffId
            };

            // Khi nhân viên rep: Chuyển trạng thái sang Open và gán người phụ trách
            ticket.Status = SupportTicketStatus.InProgress;
            ticket.AssignedToStaffId = staffId;

            await _supportRepo.AddReplyAsync(reply);
            await _supportRepo.UpdateTicketAsync(ticket);
        }

        public async Task UserReplyAsync(int ticketId, string message, string userId)
        {
            var ticket = await _supportRepo.GetTicketByIdAsync(ticketId);
            if (ticket == null || ticket.SubmittedByUserId != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền phản hồi ticket này.");

            var reply = new SupportTicketReply
            {
                TicketId = ticketId,
                Message = message,
                RepliedByUserId = userId
            };

            await _supportRepo.AddReplyAsync(reply);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId)
        {
            var tickets = await _supportRepo.GetTicketsByUserIdAsync(userId);
            return tickets.Select(t => t.ToDto());
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllTicketsForStaffAsync()
        {
            var tickets = await _supportRepo.GetAllTicketsAsync();
            return tickets.Select(t => t.ToDto());
        }

        public async Task<TicketResponseDto?> GetTicketDetailsAsync(int ticketId)
        {
            var ticket = await _supportRepo.GetTicketByIdAsync(ticketId);
            return ticket?.ToDto();
        }

        public async Task CloseTicketAsync(int ticketId)
        {
            var ticket = await _supportRepo.GetTicketByIdAsync(ticketId);
            if (ticket != null)
            {
                ticket.Status = SupportTicketStatus.Closed;
                await _supportRepo.UpdateTicketAsync(ticket);
            }
        }
    }
}