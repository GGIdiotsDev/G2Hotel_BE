using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class PaymentDTO
    {
        public decimal TotalPayment { get; set; }
        public int PaymentTypeId { get; set; }
        public int CustomerId { get; set; }
        public long OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }

        public long PaymentTranId { get; set; }
        public string? BankCode { get; set; }
        public string? PayStatus { get; set; }
    }
}