using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IDetailRoomPaymentRepository
    {
        Task<IEnumerable<DetailRoomPayment>> GetDetailRoomPaymentsByPaymentIdAsync(int paymentId);
        void RemoveDetailRoomPayment(DetailRoomPayment detailRoomPayment);
        void RemoveDetailRoomPaymentsRange(IEnumerable<DetailRoomPayment> detailRoomPayments);
    }
}