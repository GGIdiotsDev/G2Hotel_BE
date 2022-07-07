using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class PaymentCacheDTO
    {
        public PaymentDTO? paymentDTO { get; set; }
        public VNPayDTO? vNPayDTO { get; set; }
    }
}