using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? PublicId { get; set; }
        public bool IsMain { get; set; }
    }
}