using Application.DTOs.Chat;

namespace Application.Interfaces
{
    public interface IChatService
    {
        Task<int> GetOrCreateConversationIdAsync(string customerId, string ownerId);
        Task<MessageDto> SendMessageAsync(int conversationId, string senderId, string message);
        Task<IEnumerable<ConversationDto>> GetMyConversationsAsync(string userId);
        Task<IEnumerable<MessageDto>> GetChatHistoryAsync(int conversationId, string userId, int skip = 0, int take = 50);
        Task MarkAsReadAsync(int conversationId, string userId);
    }
}