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
            CreateMap<RoomUpdateDTO, Room>();
            CreateMap<PaymentTypeDTO, PaymentType>();
            CreateMap<PaymentType, PaymentTypeDTO>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<AppUser, UserDTO>();
            CreateMap<AppUser, MemberDTO>();
        }
    }
}