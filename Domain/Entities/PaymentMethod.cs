using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal PaymentCost { get; set; }

        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }

    }
}
