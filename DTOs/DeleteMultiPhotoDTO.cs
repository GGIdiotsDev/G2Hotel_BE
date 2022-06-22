using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class DeleteMultiPhotoDTO
    {
        public int roomId { get; set; }
        public ICollection<PhotoDTO> photos { get; set; } = null!;
    }
}