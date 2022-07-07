using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("Payments")]
    public class Payment : AbstractEntity
    {
        public int Id { get; set; }
        public decimal TotalPayment { get; set; }
        public int PaymentTypeId { get; set; }
        public int? CustomerId { get; set; }
        public long OrderId { get; set; }
        public long PaymentTranId { get; set; }

        public string? BankCode { get; set; }
        public string? PayStatus { get; set; }
        public PaymentType? PaymentType { get; set; }
        public Customer? Customer { get; set; }
        public IList<DetailServicePayment>? DetailServicePayments { get; set; }
        public IList<DetailRoomPayment>? DetailRoomPayments { get; set; }
    }
}