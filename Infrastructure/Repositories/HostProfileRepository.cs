using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HostProfileRepository : IHostProfileRepository
    {
        private readonly AppDbContext _context;

        public HostProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateHostProfileAsync(HostProfile hostProfile)
        {
            _context.HostProfiles.Add(hostProfile);
            await _context.SaveChangesAsync();

        }

        public async Task<HostProfile> GetHostProfileByIdAsync(int hostProfileId)
        {
            return await _context.HostProfiles.FindAsync(hostProfileId);
        }

        public async Task<HostProfile> GetHostProfileByUserIdAsync(string userId)
        {
            return await _context.HostProfiles
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task UpdateHostProfileAsync(HostProfile hostProfile)
        {   
            _context.HostProfiles.Update(hostProfile);
            await _context.SaveChangesAsync();
        }
    }
}
