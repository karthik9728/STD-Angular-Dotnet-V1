using AutoMapper;
using DatingApp.Application.DTO.Photo;
using DatingApp.Application.DTO.User;
using DatingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser,AppUserDto>().ReverseMap();
            CreateMap<Photo,PhotoDto>().ReverseMap();
        }
    }
}
