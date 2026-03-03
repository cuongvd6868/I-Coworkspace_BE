using Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<NewUserDto> RegisterAsync(RegisterDto registerDto);
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
    }
}
