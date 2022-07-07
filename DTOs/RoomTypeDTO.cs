using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<RoomDTO>? Rooms { get; set; }
        public ICollection<PhotoDTO>? Photos { get; set; }
    }
}