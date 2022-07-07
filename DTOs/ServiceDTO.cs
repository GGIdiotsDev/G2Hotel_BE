using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal DefaultPrice { get; set; }
        public decimal? PromotionPrice { get; set; }
    }
}