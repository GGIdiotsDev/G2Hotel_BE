using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DateFilterDTO
    {
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
    }
}