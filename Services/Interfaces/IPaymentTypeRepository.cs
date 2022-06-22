using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IPaymentTypeRepository
    {
        void AddPaymentType(PaymentType paymentType);
    }
}