using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class SupportHub : Hub
    {
        // Hàm để người dùng tham gia vào "phòng chat" của riêng Ticket đó
        public async Task JoinTicketGroup(int ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Ticket_{ticketId}");
        }

        // Hàm để rời phòng
        public async Task LeaveTicketGroup(int ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Ticket_{ticketId}");
        }
    }
}