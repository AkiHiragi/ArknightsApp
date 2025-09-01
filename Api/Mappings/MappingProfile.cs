using ArknightsApp.ModelDto;
using ArknightsApp.Models;
using AutoMapper;

namespace ArknightsApp.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OperatorDto, Operator>();
        CreateMap<Operator, OperatorDto>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
            .ForMember(dest => dest.SubclassName, opt => opt.MapFrom(src => src.Subclass.Name));

        CreateMap<Class, ClassDto>();

        CreateMap<Subclass, SubclassDto>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));
        CreateMap<CreateSubclassDto, Subclass>();
    }
}