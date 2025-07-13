using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;

namespace ArknightsApp.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
           .ForMember(dest =>
                          dest.Role, opt =>
                            opt.MapFrom(src => src.Role.ToString()));
    }
}