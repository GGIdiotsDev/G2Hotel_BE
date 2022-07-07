using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IPaymentRepository
    {
        Payment AddPayment(Payment payment);
        void AddDetailRoomPayment(int paymentId, List<Room> rooms, DateTime checkInDate, DateTime checkOutDate);
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<Payment> GetPaymentByOrderIdAsync(long orderId);
        Task<Payment> GetDetailPaymentByOrderIdAsync(long orderId);

    }
}