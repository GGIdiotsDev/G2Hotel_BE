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
    public class DetailRoomPaymentRepository : IDetailRoomPaymentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DetailRoomPaymentRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void RemoveDetailRoomPayment(DetailRoomPayment detailRoomPayment)
        {
            _context.DetailRoomPayments.Remove(detailRoomPayment);
        }

        public async Task<IEnumerable<DetailRoomPayment>> GetDetailRoomPaymentsByPaymentIdAsync(int paymentId)
        {
            return await _context.DetailRoomPayments.Where(x => x.PaymentId == paymentId).ToListAsync();
        }

        public void RemoveDetailRoomPaymentsRange(IEnumerable<DetailRoomPayment> detailRoomPayments)
        {
            _context.DetailRoomPayments.RemoveRange(detailRoomPayments);
        }
    }
}