using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;

namespace g2hotel_server.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Photo, PhotoDTO>();
            CreateMap<Room, RoomDTO>();
            CreateMap<RoomDTO, Room>();
            CreateMap<Room, RoomSelectedDTO>();
            CreateMap<Room, RoomDetailPaymentDTO>();
            CreateMap<RoomSelectedDTO, Room>();
            CreateMap<RoomUpdateDTO, Room>();
            CreateMap<PaymentTypeDTO, PaymentType>();
            CreateMap<PaymentType, PaymentTypeDTO>();
            CreateMap<PaymentDTO, Payment>();
            CreateMap<Payment, PaymentDTO>();
            //
            CreateMap<Payment, DetailPaymentDTO>()
                .ForMember(dest => dest.OrderId,
                opt => opt.MapFrom(src => src.OrderId.ToString()));
            //
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<AppUser, UserDTO>();
            CreateMap<AppUser, MemberDTO>();
            CreateMap<AppUser, UserUpdateDTO>();
            CreateMap<UserUpdateDTO, AppUser>();
            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceDTO, Service>();
            CreateMap<Service, ServiceDetailPaymentDTO>();
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<RoomTypeDTO, RoomType>();
            CreateMap<DetailRoomPayment, DetailRoomPaymentDTO>();
            CreateMap<DetailServicePayment, DetailServicePaymentDTO>();
            CreateMap<Customer, CustomerDTO>();

        }
    }
}