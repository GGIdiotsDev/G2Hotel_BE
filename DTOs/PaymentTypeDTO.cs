using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class PaymentTypeDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<PaymentDTO>? Payments { get; set; }
    }
}