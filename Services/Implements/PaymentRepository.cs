using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.Data;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;

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
        public void AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddDetailRoomPayment(int paymentId, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate, int amount)
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
                            Amount = amount
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

        public Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Payment payment)
        {
            _context.Payments.Remove(payment);
        }
    }
}