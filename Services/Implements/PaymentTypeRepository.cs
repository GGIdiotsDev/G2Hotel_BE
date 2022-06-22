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
    public class PaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PaymentTypeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddPaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.Add(paymentType);
        }
    }
}