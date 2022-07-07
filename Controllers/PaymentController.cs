using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace g2hotel_server.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //API: Get all payments
        [HttpGet]
        public async Task<IEnumerable<DetailPaymentDTO>> GetAllPayments()
        {
            var listPayments = await _unitOfWork.PaymentRepository.GetPaymentsAsync();

            return _mapper.Map<IEnumerable<DetailPaymentDTO>>(listPayments);
        }

        //API: Get all payments
        [HttpGet("detail/{orderId}")]
        public async Task<DetailPaymentDTO> GetDetailPaymentByOrderId(long orderId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetDetailPaymentByOrderIdAsync(orderId);
            if (payment.DetailRoomPayments != null)
            {
                foreach (var detailRoomPayment in payment.DetailRoomPayments)
                {
                    detailRoomPayment.Room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(detailRoomPayment.RoomId);
                }
            }
            if (payment.DetailServicePayments != null)
            {
                foreach (var detailServicePayment in payment.DetailServicePayments)
                {
                    detailServicePayment.Service = await _unitOfWork.ServiceRepository.GetServiceById(detailServicePayment.ServiceId);
                }
            }

            return _mapper.Map<DetailPaymentDTO>(payment);
        }
    }
}