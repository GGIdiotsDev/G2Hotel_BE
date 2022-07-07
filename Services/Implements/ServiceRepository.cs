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
    public class ServiceRepository : IServiceRepository

    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ServiceRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Service AddService(Service service)
        {
            return _context.Services.Add(service).Entity;
        }

        public void Delete(Service service)
        {
            _context.Remove(service);
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _context.Services.FindAsync(id) ?? throw new Exception("Service not found");
        }
        public async Task<IEnumerable<Service>> GetServiceTasksAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public void Update(Service service)
        {
            _context.Entry(service).State = EntityState.Modified;
        }

    }
}