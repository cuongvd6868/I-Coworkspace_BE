using Application.DTOs.Chat;
using Domain.Entities;

namespace Application.Mappings
{
    public static class ChatMappingExtensions
    {
        public static MessageDto ToDto(this ChatMessage entity)
        {
            return new MessageDto
            {
                Id = entity.Id,
                Message = entity.Message,
                SenderId = entity.SenderId,
                SentAt = entity.SentAt,
                IsRead = entity.IsRead
            };
        }

        public static ConversationDto ToDto(this Conversation entity, string currentUserId)
        {
            var isCustomer = entity.CustomerId == currentUserId;
            var otherUser = isCustomer ? entity.Owner : entity.Customer;

            return new ConversationDto
            {
                Id = entity.Id,
                OtherUserId = otherUser?.Id ?? "",
                OtherUserName = otherUser?.UserName ?? "Unknown",
                LastMessageAt = entity.LastMessageAt,
                LastMessage = entity.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.Message ?? ""
            };
        }
    }
}