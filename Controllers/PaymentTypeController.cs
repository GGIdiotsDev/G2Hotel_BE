using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace g2hotel_server.Controllers
{
    public class PaymentTypeController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentTypeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("add-payment-type")]
        //add new payment type
        public async Task<IActionResult> AddPaymentType(PaymentTypeDTO paymentTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentType = _mapper.Map<PaymentType>(paymentTypeDTO);

            _unitOfWork.PaymentTypeRepository.AddPaymentType(paymentType);

            if (!await _unitOfWork.Complete())
            {
                return BadRequest("Failed to add payment type");
            }
            return Ok(paymentType);
        }
    }
}
