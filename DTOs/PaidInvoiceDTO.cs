using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class PaidInvoiceDTO
    {
        public string? vnp_ResponseCode { get; set; }
        public long vnp_TxnRef { get; set; }
    }
}