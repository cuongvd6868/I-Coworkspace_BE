using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account
{
    public class NewUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
