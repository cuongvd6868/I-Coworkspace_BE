using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISupportRepository
    {
        // Ticket
        Task<SupportTicket?> GetTicketByIdAsync(int id);
        Task<IEnumerable<SupportTicket>> GetTicketsByUserIdAsync(string userId);
        Task<IEnumerable<SupportTicket>> GetAllTicketsAsync();
        Task AddTicketAsync(SupportTicket ticket);
        Task UpdateTicketAsync(SupportTicket ticket);

        // Reply
        Task AddReplyAsync(SupportTicketReply reply);
    }
}