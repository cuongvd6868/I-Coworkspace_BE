using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Address 
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Ward { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Country { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
