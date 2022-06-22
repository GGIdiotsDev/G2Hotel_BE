using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.Data;
using g2hotel_server.Services.Interfaces;

namespace g2hotel_server.Services.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IRoomRepository RoomRepository => new RoomRepository(_context, _mapper);

        public IPaymentRepository PaymentRepository => new PaymentRepository(_context, _mapper);

        public IPaymentTypeRepository PaymentTypeRepository => new PaymentTypeRepository(_context, _mapper);

        public IPhotoRepository PhotoRepository => new PhotoRepository(_context, _mapper);

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}