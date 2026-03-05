using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.HostProfile
{
    public class HostProfileResponseDto
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public string? ContactPhone { get; set; }
        public string? Avatar { get; set; }
        public string? CoverPhoto { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
