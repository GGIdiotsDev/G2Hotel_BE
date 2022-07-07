using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DetailRoomPaymentDTO
    {
        public int RoomId { get; set; }
        public RoomDetailPaymentDTO Room { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Amount { get; set; }
    }
}