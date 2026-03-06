using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion?> GetByIdAsync(int id);
        Task<Promotion?> GetByCodeAsync(string code);
        Task<IEnumerable<Promotion>> GetAllAsync();
        // Tìm mã hợp lệ cho một Workspace cụ thể (Xử lý logic Admin vs Owner)
        Task<Promotion?> GetValidPromotionByWorkspaceAsync(string code, int workspaceId);
        Task AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task DeleteAsync(int id);
    }
}