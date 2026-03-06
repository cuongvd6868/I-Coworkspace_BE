using Application.DTOs.Support;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepo;
        private readonly IHubContext<SupportHub> _hubContext;

        public SupportService(ISupportRepository supportRepo, IHubContext<SupportHub> hubContext)
        {
            _supportRepo = supportRepo;
            _hubContext = hubContext;
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
            if (ticket.Status == SupportTicketStatus.Closed) throw new Exception("Ticket này đã đóng, không thể phản hồi.");

            var reply = new SupportTicketReply
            {
                TicketId = ticketId,
                Message = message,
                RepliedByUserId = staffId
            };

            // Cập nhật trạng thái và người phụ trách
            ticket.Status = SupportTicketStatus.InProgress;
            ticket.AssignedToStaffId = staffId;

            await _supportRepo.AddReplyAsync(reply);
            await _supportRepo.UpdateTicketAsync(ticket);

            // Gửi SignalR real-time cho cả Group (bao gồm User đang xem)
            await SendSignalRReply(ticketId, message, "Staff");
        }

        public async Task UserReplyAsync(int ticketId, string message, string userId)
        {
            var ticket = await _supportRepo.GetTicketByIdAsync(ticketId);

            if (ticket == null) throw new Exception("Ticket không tồn tại.");
            if (ticket.SubmittedByUserId != userId) throw new UnauthorizedAccessException("Bạn không có quyền phản hồi ticket này.");
            if (ticket.Status == SupportTicketStatus.Closed) throw new Exception("Ticket này đã đóng, vui lòng tạo ticket mới nếu cần hỗ trợ thêm.");

            var reply = new SupportTicketReply
            {
                TicketId = ticketId,
                Message = message,
                RepliedByUserId = userId
            };

            await _supportRepo.AddReplyAsync(reply);

            // Nếu ticket đang ở trạng thái New, có thể giữ nguyên hoặc chuyển sang Open tùy logic
            // Ở đây ta giữ nguyên để Staff biết User vừa phản hồi thêm vào yêu cầu gốc

            // Gửi SignalR real-time cho cả Group (bao gồm Staff đang trực)
            await SendSignalRReply(ticketId, message, "Customer");
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

                // Thông báo real-time rằng ticket đã đóng
                await _hubContext.Clients.Group($"Ticket_{ticketId}").SendAsync("TicketClosed", ticketId);
            }
        }

        // Hàm helper dùng chung để gửi SignalR tránh lặp code
        private async Task SendSignalRReply(int ticketId, string message, string senderType)
        {
            await _hubContext.Clients.Group($"Ticket_{ticketId}")
                .SendAsync("ReceiveReply", new
                {
                    TicketId = ticketId,
                    Message = message,
                    RepliedBy = senderType,
                    CreatedAt = DateTime.Now
                });
        }
    }
}