using Application.DTOs.Account;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<NewUserDto> RegisterAsync(RegisterDto registerDto)
        {
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!createUser.Succeeded)
                throw new Exception(string.Join(", ", createUser.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRoleAsync(appUser, "Customer");

            if (!roleResult.Succeeded)
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            return new NewUserDto
            {
                Username = appUser.UserName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new UnauthorizedAccessException("Invalid username or password");

            return new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
