using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using g2hotel_server.Data;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace g2hotel_server.Services.Implements
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RoomRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Room AddRoom(Room room)
        {
            return _context.Rooms.Add(room).Entity;
        }

        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
        }

        public async Task<Room> GetRoomByCodeAsync(string code)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Code == code) ?? throw new Exception("Room not found");
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.Include(p => p.Photos).Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Room not found");
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.Rooms
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByCheckInDateAsync(DateTime checkInDate, DateTime checkOutDate)
        {
            //set time to 14:00:00 for check in date
            checkInDate = checkInDate.Date.AddHours(14);
            var listRooms = await _context.Rooms.Include(r => r.DetailRoomPayments).ToListAsync();
            //loop through list room available and add amount of detail room payment to room with condition checkInDate >= detail room payment.CheckOutDate
            foreach (var room in listRooms)
            {
                if (room.DetailRoomPayments != null)
                {
                    var detailRoomPayments = room.DetailRoomPayments.Where(x => x.CheckOutDate <= checkInDate).ToList();
                    foreach (var detailRoomPayment in detailRoomPayments)
                    {
                        room.Amount += detailRoomPayment.Amount;
                    }
                }
            }

            return listRooms;
        }

        public async Task<bool> RoomCodeExists(string code)
        {
            return await _context.Rooms.AnyAsync(x => x.Code == code);
        }

        public void Update(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
        }
    }
}