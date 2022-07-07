using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DetailServicePaymentDTO
    {
        public int ServiceId { get; set; }
        public ServiceDetailPaymentDTO Service { get; set; } = null!;
        public int Amount { get; set; }
    }
}