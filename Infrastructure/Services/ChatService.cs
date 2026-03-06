using Application.DTOs.Chat;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IChatRepository chatRepo, IHubContext<ChatHub> hubContext)
        {
            _chatRepo = chatRepo;
            _hubContext = hubContext;
        }

        public async Task<int> GetOrCreateConversationIdAsync(string customerId, string ownerId)
        {
            // Kiểm tra xem đã có cuộc hội thoại giữa 2 người này chưa
            var conv = await _chatRepo.GetConversationByParticipantsAsync(customerId, ownerId);
            if (conv != null) return conv.Id;

            // Nếu chưa, tạo mới
            var newConv = new Conversation
            {
                CustomerId = customerId,
                OwnerId = ownerId,
                LastMessageAt = DateTime.Now
            };

            await _chatRepo.CreateConversationAsync(newConv);
            return newConv.Id;
        }

        public async Task<MessageDto> SendMessageAsync(int conversationId, string senderId, string message)
        {
            var conversation = await _chatRepo.GetConversationAsync(conversationId);
            if (conversation == null) throw new Exception("Cuộc hội thoại không tồn tại.");

            // Tạo tin nhắn mới
            var chatMsg = new ChatMessage
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _chatRepo.AddMessageAsync(chatMsg);

            // Chuyển thành DTO để gửi đi
            var messageDto = chatMsg.ToDto();

            // Bắn SignalR tới Group chat
            await _hubContext.Clients.Group($"Chat_{conversationId}")
                .SendAsync("ReceiveChatMessage", messageDto);

            return messageDto;
        }

        public async Task<IEnumerable<ConversationDto>> GetMyConversationsAsync(string userId)
        {
            var conversations = await _chatRepo.GetUserConversationsAsync(userId);
            return conversations.Select(c => c.ToDto(userId));
        }

        public async Task<IEnumerable<MessageDto>> GetChatHistoryAsync(int conversationId, string userId, int skip = 0, int take = 50)
        {
            // Kiểm tra quyền (phải là người trong cuộc mới xem được history)
            var conv = await _chatRepo.GetConversationAsync(conversationId);
            if (conv == null || (conv.CustomerId != userId && conv.OwnerId != userId))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xem lịch sử chat này.");
            }

            var messages = await _chatRepo.GetMessagesByConversationIdAsync(conversationId, skip, take);

            // Sắp xếp lại tin nhắn cũ -> mới để hiển thị lên UI
            return messages.Select(m => m.ToDto()).OrderBy(m => m.SentAt);
        }

        public async Task MarkAsReadAsync(int conversationId, string userId)
        {
            await _chatRepo.MarkMessagesAsReadAsync(conversationId, userId);

            // Gửi SignalR thông báo tin nhắn đã được đọc (tùy chọn)
            await _hubContext.Clients.Group($"Chat_{conversationId}")
                .SendAsync("MessagesRead", new { conversationId, readerId = userId });
        }
    }
}