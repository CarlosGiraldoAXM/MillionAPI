using AutoMapper;
using MillionAPI.Application.DTOs;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Application.Profiles;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        // Mapeo de Entity a DTO
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Name : string.Empty))
            .ForMember(dest => dest.ImagesCount, opt => opt.MapFrom(src => src.Images.Count));

        // Mapeo de CreateDto a Entity
        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore());

        // Mapeo de UpdateDto a Entity (para actualizaciones)
        CreateMap<UpdatePropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore());
    }
}
