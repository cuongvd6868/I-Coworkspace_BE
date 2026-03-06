using System;
using System.Collections.Generic;
using System.Text;


namespace Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Thông tin người gửi
        public string SenderRole { get; set; } = string.Empty;
        public int SenderId { get; set; }

        // Thông tin Workspace liên quan (nếu là thông báo từ Owner)
        public int? WorkSpaceId { get; set; }
        public string? WorkSpaceTitle { get; set; }
    }
}