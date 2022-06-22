using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int NumBeds { get; set; }
        public int NumAdults { get; set; }
        public int NumChilds { get; set; }
        public ICollection<PhotoDTO>? Photos { get; set; }
    }
}