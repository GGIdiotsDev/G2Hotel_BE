using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("DetailRoomPayments")]
    public class DetailRoomPayment : AbstractEntity
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Amount { get; set; }
    }
}