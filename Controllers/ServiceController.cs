using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace g2hotel_server.Controllers
{
    public class ServiceController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-services")]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetSevices()
        {
            var servicesEntity = await _unitOfWork.ServiceRepository.GetServiceTasksAsync();
            var services = _mapper.Map<IEnumerable<ServiceDTO>>(servicesEntity);
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDTO>> GetServiceById(int id)
        {
            var serviceEntity = await _unitOfWork.ServiceRepository.GetServiceById(id);
            var service = _mapper.Map<ServiceDTO>(serviceEntity);
            return Ok(service);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPost]
        public async Task<ActionResult<ServiceDTO>> AddService(ServiceDTO serviceDTO)
        {
            var servicesEntity = _mapper.Map<Service>(serviceDTO);
            var result = _unitOfWork.ServiceRepository.AddService(servicesEntity);

            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem adding service");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpDelete("delete-service/{id}")]
        public async Task<ActionResult<ServiceDTO>> DeleteService(int id)
        {
            var servicesEntity = await _unitOfWork.ServiceRepository.GetServiceById(id);
            if (servicesEntity == null)
            {
                return NotFound();
            }
            _unitOfWork.ServiceRepository.Delete(servicesEntity);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Problem deleting service");
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPut]
        public async Task<ActionResult<ServiceDTO>> UpdateService(ServiceDTO serviceDTO)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceById(serviceDTO.Id);
            _mapper.Map(serviceDTO, service);
            _unitOfWork.ServiceRepository.Update(service);

            if (service == null)
                return NotFound($"Service with Id = {serviceDTO.Id} not found");
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update service");
        }
    }

}