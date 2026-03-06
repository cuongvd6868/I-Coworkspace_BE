using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SupportRepository : ISupportRepository
    {
        private readonly AppDbContext _context;

        public SupportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SupportTicket?> GetTicketByIdAsync(int id)
        {
            return await _context.SupportTickets
                .Include(t => t.SubmittedByUser)
                .Include(t => t.AssignedToStaff)
                .Include(t => t.Replies)
                    .ThenInclude(r => r.RepliedByUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<SupportTicket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.SupportTickets
                .Include(t => t.Replies)
                .Where(t => t.SubmittedByUserId == userId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportTicket>> GetAllTicketsAsync()
        {
            return await _context.SupportTickets
                .Include(t => t.SubmittedByUser)
                .OrderByDescending(t => t.Status == Domain.Enums.SupportTicketStatus.New)
                .ToListAsync();
        }

        public async Task AddTicketAsync(SupportTicket ticket)
        {
            await _context.SupportTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(SupportTicket ticket)
        {
            _context.SupportTickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AddReplyAsync(SupportTicketReply reply)
        {
            await _context.SupportTicketReplies.AddAsync(reply);
            await _context.SaveChangesAsync();
        }
    }
}