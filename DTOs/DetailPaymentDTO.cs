using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DetailPaymentDTO
    {
        public int Id { get; set; }
        public decimal TotalPayment { get; set; }
        public string? OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? PayStatus { get; set; }
        public PaymentTypeDTO? PaymentType { get; set; }
        public CustomerDTO? Customer { get; set; }
        public IList<DetailServicePaymentDTO>? DetailServicePayments { get; set; }
        public IList<DetailRoomPaymentDTO>? DetailRoomPayments { get; set; }
    }
}