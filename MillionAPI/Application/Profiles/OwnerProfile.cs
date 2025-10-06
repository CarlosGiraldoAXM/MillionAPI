using AutoMapper;
using MillionAPI.Application.DTOs;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Application.Profiles;

public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        // Mapeo de Entity a DTO
        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties.Count));

    }
}
