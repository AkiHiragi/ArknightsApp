using ArknightsApp.ModelDto;
using ArknightsApp.Models;
using AutoMapper;

namespace ArknightsApp.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OperatorDto, Operator>();
        CreateMap<Operator, OperatorDto>();
    }
}