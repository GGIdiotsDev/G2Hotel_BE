using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        void AddDetailRoomPayment(int paymentId, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate, int amount);
        void Update(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
    }
}