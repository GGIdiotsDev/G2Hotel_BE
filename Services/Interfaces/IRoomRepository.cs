using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IRoomRepository
    {
        Room AddRoom(Room room);
        void Update(Room room);
        void Delete(Room room);
        Task<bool> RoomCodeExists(string code);
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> GetRoomByCodeAsync(string code);
    }
}