using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string? Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
