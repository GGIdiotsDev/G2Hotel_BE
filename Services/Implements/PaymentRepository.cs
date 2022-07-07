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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PaymentRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Payment AddPayment(Payment payment)
        {
            return _context.Payments.Add(payment).Entity;
        }

        public void AddDetailRoomPayment(int paymentId, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
        {
            if (_context.Payments.Find(paymentId) == null)
            {
                throw new Exception("Payment not found");
            }
            else
            {
                foreach (var room in rooms)
                {
                    if (_context.Rooms.Find(room.Id) == null)
                    {
                        throw new Exception("Room not found");
                    }
                    else
                    {
                        var detailRoomPayment = new DetailRoomPayment
                        {
                            PaymentId = paymentId,
                            RoomId = room.Id,
                            CheckInDate = checkInDate,
                            CheckOutDate = checkOutDate,
                            Amount = room.Amount
                        };
                        _context.DetailRoomPayments.Add(detailRoomPayment);
                    }

                }
            }
        }


        public Task<Payment> GetPaymentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(long orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(r => r.OrderId == orderId) ?? throw new Exception("Payment not found");
        }

        public async Task<Payment> GetDetailPaymentByOrderIdAsync(long orderId)
        {
            return await _context.Payments.Where(p => p.OrderId == orderId)
            .Include(d => d.DetailRoomPayments)
            .Include(d => d.DetailServicePayments)
            .Include(c => c.Customer)
            .Include(pmt => pmt.PaymentType)
            .FirstOrDefaultAsync() ?? throw new Exception("Payment not found");
        }
    }
}