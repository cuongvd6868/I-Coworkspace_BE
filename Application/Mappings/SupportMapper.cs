using Application.DTOs.Support;
using Domain.Entities;

public static class SupportMapper
{
    public static TicketResponseDto ToDto(this SupportTicket entity)
    {
        return new TicketResponseDto
        {
            Id = entity.Id,
            Subject = entity.Subject,
            Message = entity.Message,
            Status = entity.Status.ToString(),
            CreatedAt = entity.CreatedAt, // <--- Bổ sung trường này
            SubmittedByUserName = entity.SubmittedByUser?.UserName ?? "Customer",
            Replies = entity.Replies?.Select(r => r.ToDto()).OrderBy(r => r.CreatedAt).ToList() ?? new()
        };
    }

    public static ReplyResponseDto ToDto(this SupportTicketReply entity)
    {
        if (entity == null) return null!;
        return new ReplyResponseDto
        {
            Id = entity.Id,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            RepliedByUserName = entity.RepliedByUser?.UserName ?? "Staff",
            UserId = entity.RepliedByUserId
        };
    }
}