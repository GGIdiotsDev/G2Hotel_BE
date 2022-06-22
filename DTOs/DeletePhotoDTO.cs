using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DeletePhotoDTO
    {
        public int photoId { get; set; }
        public int roomId { get; set; }
    }
}