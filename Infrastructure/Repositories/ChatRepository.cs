using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context) => _context = context;

        public async Task<Conversation?> GetConversationAsync(int id)
        {
            return await _context.Conversations
                .Include(c => c.Customer)
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Conversation?> GetConversationByParticipantsAsync(string customerId, string ownerId)
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.OwnerId == ownerId);
        }

        public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId)
        {
            return await _context.Conversations
                .Include(c => c.Customer)
                .Include(c => c.Owner)
                .Where(c => c.CustomerId == userId || c.OwnerId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task CreateConversationAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByConversationIdAsync(int conversationId, int skip, int take)
        {
            return await _context.ChatMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task MarkMessagesAsReadAsync(int conversationId, string readerId)
        {
            var unreadMessages = await _context.ChatMessages
                .Where(m => m.ConversationId == conversationId && m.SenderId != readerId && !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages) msg.IsRead = true;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(int conversationId, string userId)
        {
            return await _context.ChatMessages
                .CountAsync(m => m.ConversationId == conversationId
                            && m.SenderId != userId
                            && !m.IsRead);
        }
    }
}