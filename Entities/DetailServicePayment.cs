using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("DetailServicePayments")]
    public class DetailServicePayment : AbstractEntity
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
        public int ServiceId { get; set; }
        public Service Services { get; set; } = null!;
    }
}