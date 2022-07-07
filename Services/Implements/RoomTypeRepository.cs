using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.Data;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace g2hotel_server.Services.Implements
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RoomTypeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public RoomType AddRoomType(RoomType roomType)
        {
            return _context.RoomTypes.Add(roomType).Entity;
        }

        public void Delete(RoomType roomType)
        {
            _context.Remove(roomType);
        }

        public async Task<RoomType> GetRoomTypeById(int id)
        {
            return await _context.RoomTypes.Include(p => p.Photos).Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Room Types not found");
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypesAsync()
        {
            return await _context.RoomTypes
            .Include(r => r.Rooms)
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public void Update(RoomType roomType)
        {
            _context.Entry(roomType).State = EntityState.Modified;
        }
    }
}