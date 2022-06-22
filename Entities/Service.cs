using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("Services")]
    public class Service : AbstractEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal DefaultPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public IList<DetailServicePayment>? DetailServicePayments { get; set; }
    }
}