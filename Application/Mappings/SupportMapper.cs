using Application.DTOs.Support;
using Domain.Entities;

public static class SupportMapper
{
    public static TicketResponseDto ToDto(this SupportTicket entity)
    {
        if (entity == null) return null!;
        return new TicketResponseDto
        {
            Id = entity.Id,
            Subject = entity.Subject,
            Message = entity.Message,
            Status = entity.Status.ToString(),
            TicketType = entity.TicketType.ToString(),
            SubmittedByUserName = entity.SubmittedByUser?.UserName ?? "Customer",
            // Ánh xạ danh sách Replies bên trong
            Replies = entity.Replies?.Select(r => r.ToDto()).ToList() ?? new()
        };
    }

    public static ReplyResponseDto ToDto(this SupportTicketReply entity)
    {
        if (entity == null) return null!;
        return new ReplyResponseDto
        {
            Id = entity.Id,
            Message = entity.Message,
            CreatedAt = DateTime.Now, // Giả sử bạn có trường này
            RepliedByUserName = entity.RepliedByUser?.UserName ?? "Staff",
            UserId = entity.RepliedByUserId
        };
    }
}