using Application.DTOs.Chat;
using Application.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // 1. Tạo hoặc lấy hội thoại hiện có (Dùng khi nhấn nút "Chat" trên UI)
        [HttpPost("start/{ownerId}")]
        public async Task<IActionResult> StartChat(string ownerId)
        {
            var customerId = User.GetUserId();
            if (string.IsNullOrEmpty(customerId)) return Unauthorized();

            var convId = await _chatService.GetOrCreateConversationIdAsync(customerId, ownerId);
            return Ok(new { conversationId = convId });
        }

        // 2. Gửi tin nhắn (Sẽ vừa lưu DB vừa bắn SignalR qua Service)
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest req)
        {
            var senderId = User.GetUserId();
            var result = await _chatService.SendMessageAsync(req.ConversationId, senderId!, req.Message);
            return Ok(result);
        }

        // 3. Lấy danh sách Inbox (Danh sách các người mình đang chat cùng)
        [HttpGet("my-conversations")]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = User.GetUserId();
            var conversations = await _chatService.GetMyConversationsAsync(userId!);
            return Ok(conversations);
        }

        // 4. Lấy lịch sử tin nhắn của một cuộc hội thoại
        [HttpGet("history/{conversationId}")]
        public async Task<IActionResult> GetChatHistory(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var userId = User.GetUserId();
            try
            {
                var messages = await _chatService.GetChatHistoryAsync(conversationId, userId!, skip, take);
                return Ok(messages);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // 5. Đánh dấu đã đọc
        [HttpPatch("{conversationId}/read")]
        public async Task<IActionResult> MarkAsRead(int conversationId)
        {
            var userId = User.GetUserId();
            await _chatService.MarkAsReadAsync(conversationId, userId!);
            return NoContent();
        }

        [HttpGet("unread-total")]
        public async Task<IActionResult> GetTotalUnreadCount()
        {
            var userId = User.GetUserId();
            var conversations = await _chatService.GetMyConversationsAsync(userId!);
            var total = conversations.Sum(c => c.UnreadCount);
            return Ok(new { totalUnread = total });
        }
    }
}