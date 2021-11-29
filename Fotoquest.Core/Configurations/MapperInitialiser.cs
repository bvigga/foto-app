using System;
using AutoMapper;
using Fotoquest.Core.DTOs;
using Fotoquest.Data;

namespace Fotoquest.Core.Configurations
{
    public class MapperInitialiser : Profile
    {
        public MapperInitialiser()
        {
            CreateMap<Foto, FotoDTO>().ReverseMap();
            CreateMap<Foto, CreateFotoDTO>().ReverseMap();
            CreateMap<Foto, UpdateFotoDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
