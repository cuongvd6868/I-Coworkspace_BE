using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Conversation?> GetConversationAsync(int id);
        Task<Conversation?> GetConversationByParticipantsAsync(string customerId, string ownerId);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
        Task CreateConversationAsync(Conversation conversation);

        Task AddMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetMessagesByConversationIdAsync(int conversationId, int skip, int take);
        Task MarkMessagesAsReadAsync(int conversationId, string readerId);
        Task<int> GetUnreadCountAsync(int conversationId, string userId);
    }
}