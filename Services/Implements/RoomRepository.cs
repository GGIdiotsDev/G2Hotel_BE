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