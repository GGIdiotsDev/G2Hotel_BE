using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IRoomTypeRepository
    {
        RoomType AddRoomType(RoomType roomType);
        void Update(RoomType roomType);
        void Delete(RoomType roomType);
        Task<IEnumerable<RoomType>> GetRoomTypesAsync();
        Task<RoomType> GetRoomTypeById(int id);
    }
}