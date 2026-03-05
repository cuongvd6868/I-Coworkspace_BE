using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHostProfileRepository
    {

        Task<HostProfile> GetHostProfileByUserIdAsync(string userId);
        Task<HostProfile> GetHostProfileByIdAsync(int hostProfileId);
        Task UpdateHostProfileAsync(HostProfile hostProfile);
        Task CreateHostProfileAsync(HostProfile hostProfile);
    }
}
