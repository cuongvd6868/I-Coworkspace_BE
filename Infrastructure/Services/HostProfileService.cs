using Application.DTOs.HostProfile;
using Application.Interfaces;
using Application.Mappings; 
using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HostProfileService : IHostProfileService
    {
        private readonly IHostProfileRepository _hostProfileRepository;

        public HostProfileService(IHostProfileRepository hostProfileRepository)
        {
            _hostProfileRepository = hostProfileRepository;
        }

        public async Task<HostProfileResponseDto> GetHostProfileByIdAsync(int hostProfileId)
        {
            var entity = await _hostProfileRepository.GetHostProfileByIdAsync(hostProfileId);
            return entity.ToDto();
        }

        public async Task<HostProfileResponseDto> GetHostProfileByUserIdAsync(string userId)
        {
            var entity = await _hostProfileRepository.GetHostProfileByUserIdAsync(userId);
            return entity.ToDto();
        }

        public async Task CreateHostProfileAsync(HostProfile hostProfile, string userId)
        {
            var existing = await _hostProfileRepository.GetHostProfileByUserIdAsync(userId);
            if (existing != null)
            {
                throw new InvalidOperationException("User already has a host profile.");
            }

            hostProfile.UserId = userId;

            await _hostProfileRepository.CreateHostProfileAsync(hostProfile);
        }

        public async Task UpdateHostProfileAsync(HostProfile hostProfile, string userId)
        {
            var existingProfile = await _hostProfileRepository.GetHostProfileByUserIdAsync(userId);

            if (existingProfile == null)
            {
                throw new KeyNotFoundException("Host profile not found.");
            }

            existingProfile.CompanyName = hostProfile.CompanyName;
            existingProfile.Description = hostProfile.Description;
            existingProfile.ContactPhone = hostProfile.ContactPhone;
            existingProfile.Avatar = hostProfile.Avatar;
            existingProfile.CoverPhoto = hostProfile.CoverPhoto;


            await _hostProfileRepository.UpdateHostProfileAsync(existingProfile);
        }
    }
}