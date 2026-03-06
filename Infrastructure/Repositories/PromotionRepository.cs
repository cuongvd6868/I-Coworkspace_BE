using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _context;

        public PromotionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Promotion?> GetByIdAsync(int id)
        {
            return await _context.Promotions
                .Include(p => p.WorkSpacePromotions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Promotion?> GetByCodeAsync(string code)
        {
            return await _context.Promotions
                .Include(p => p.WorkSpacePromotions)
                .FirstOrDefaultAsync(p => p.Code == code.ToUpper());
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _context.Promotions
                .Include(p => p.WorkSpacePromotions)
                .ToListAsync();
        }

        public async Task<Promotion?> GetValidPromotionByWorkspaceAsync(string code, int workspaceId)
        {
            var now = DateTime.Now;
            return await _context.Promotions
                .Include(p => p.WorkSpacePromotions)
                .FirstOrDefaultAsync(p => p.Code == code.ToUpper()
                    && p.IsActive
                    && p.StartDate <= now
                    && p.EndDate >= now
                    && (p.UsageLimit == 0 || p.UsedCount < p.UsageLimit)
                    // Logic: Mã Admin (HostId null) HOẶC Mã Owner có liên kết với WorkspaceId này
                    && (p.HostId == null || p.WorkSpacePromotions.Any(wp => wp.WorkSpaceId == workspaceId)));
        }

        public async Task AddAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }
    }
}