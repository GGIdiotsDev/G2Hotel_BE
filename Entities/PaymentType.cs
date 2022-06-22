using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("PaymentTypes")]
    public class PaymentType : AbstractEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}