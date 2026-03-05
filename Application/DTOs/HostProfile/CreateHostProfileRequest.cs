using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.HostProfile
{
    public class CreateHostProfileRequest
    {
        public string userId {  get; set; }
        public string? CompanyName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(1000)]
        public string? Avatar { get; set; }

        [MaxLength(1000)]
        public string? CoverPhoto { get; set; }
    }
}
