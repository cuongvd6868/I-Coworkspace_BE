using Application.DTOs.HostProfile;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public static class HostProfileMapper
    {
        public static HostProfileResponseDto ToDto(this HostProfile entity)
        {
            if (entity == null) return null!;

            return new HostProfileResponseDto
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                Description = entity.Description,
                ContactPhone = entity.ContactPhone,
                Avatar = entity.Avatar,
                CoverPhoto = entity.CoverPhoto,
                IsVerified = entity.IsVerified
            };
        }

        public static HostProfile ToHostProfileCreateDTO(this CreateHostProfileRequest entity)
        {
            if (entity == null) return null!;

            return new HostProfile
            {
                UserId = entity.userId,
                CompanyName = entity.CompanyName,
                Description = entity.Description,
                ContactPhone = entity.ContactPhone,
                Avatar = entity.Avatar,
                CoverPhoto = entity.CoverPhoto
            };
        }

        public static HostProfile ToHostProfileUpdateDTO(this UpdateHostProfileRequest entity)
        {
            if (entity == null) return null!;

            return new HostProfile
            {
                UserId = entity.userId,
                CompanyName = entity.CompanyName,
                Description = entity.Description,
                ContactPhone = entity.ContactPhone,
                Avatar = entity.Avatar,
                CoverPhoto = entity.CoverPhoto
            };
        }
    }
}
