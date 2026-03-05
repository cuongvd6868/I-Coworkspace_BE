using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.HostProfile;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IHostProfileService
    {
        Task<HostProfileResponseDto> GetHostProfileByUserIdAsync(string userId);
        Task<HostProfileResponseDto> GetHostProfileByIdAsync(int hostProfileId);
        Task UpdateHostProfileAsync(HostProfile hostProfile, string userId);
        Task CreateHostProfileAsync(HostProfile hostProfile, string userId);
    }
}
