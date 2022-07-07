using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class CancelInvoiceDTO
    {
        public long OrderId { get; set; }
        public string? StatusInvoice { get; set; }
    }
}