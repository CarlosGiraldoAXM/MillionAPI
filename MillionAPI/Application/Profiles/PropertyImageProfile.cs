using AutoMapper;
using MillionAPI.Application.DTOs;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Application.Profiles;

public class PropertyImageProfile : Profile
{
    public PropertyImageProfile()
    {
        // Mapeo de Entity a DTO
        CreateMap<PropertyImage, PropertyImageDto>();

        // Mapeo de CreateDto a Entity
        CreateMap<CreatePropertyImageDto, PropertyImage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Property, opt => opt.Ignore());

        // Mapeo de UpdateDto a Entity (para actualizaciones)
        CreateMap<UpdatePropertyImageDto, PropertyImage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyId, opt => opt.Ignore())
            .ForMember(dest => dest.Property, opt => opt.Ignore());
    }
}
