using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Chat
{
    public class SendMessageRequest
    {
        [Required]
        public int ConversationId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}