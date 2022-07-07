using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class VNPayDTO
    {
        public DateTime vnp_ExpireDate { get; set; }
        public string? vnp_Bill_Mobile { get; set; }
        public string? vnp_Bill_Email { get; set; }
        public string? vnp_Bill_FirstName { get; set; }
        public string? vnp_Bill_LastName { get; set; }

        public decimal vnp_Bill_TotalPrice { get; set; }
        public DateTime vnp_Bill_CheckIn { get; set; }
        public DateTime vnp_Bill_CheckOut { get; set; }

        public ICollection<RoomSelectedDTO>? vnp_Bill_RoomSelecteds { get; set; }
        public ICollection<ServiceSelectedDTO>? vnp_Bill_ServiceSelecteds { get; set; }

    }
}