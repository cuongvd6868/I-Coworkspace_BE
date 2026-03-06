using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // Khi người dùng mở hộp chat, họ sẽ join vào "phòng" riêng của cuộc hội thoại đó
        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{conversationId}");
        }

        public async Task LeaveConversation(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{conversationId}");
        }
    }
}
