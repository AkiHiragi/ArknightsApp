using ArknightsApp.DTO;
using ArknightsApp.Models;
using ArknightsApp.Services;
using AutoMapper;

namespace ArknightsApp.Mapping;

public class OperatorProfile : Profile
{
    public OperatorProfile()
    {
        // Маппинг основного оператора для списков
        CreateMap<Operator, OperatorDto>()
           .ForMember(dest
                          => dest.ClassName, opt
                          => opt.MapFrom(src => src.OperatorClass.Name))
           .ForMember(dest
                          => dest.SubClassName, opt
                          => opt.MapFrom(src => src.SubClass.Name))
           .ForMember(dest
                          => dest.FactionName, opt
                          => opt.MapFrom(src => src.Faction != null ? src.Faction.Name : null));

        // Маппинг детального оператора
        CreateMap<Operator, OperatorDetailDto>()
           .ForMember(dest
                          => dest.ClassName, opt
                          => opt.MapFrom(src => src.OperatorClass != null ? src.OperatorClass.Name : ""))
           .ForMember(dest
                          => dest.SubClassName, opt
                          => opt.MapFrom(src => src.SubClass != null ? src.SubClass.Name : ""))
           .ForMember(dest
                          => dest.FactionName, opt
                          => opt.MapFrom(src => src.Faction != null ? src.Faction.Name : null))
           .ForMember(dest
                          => dest.BaseStats, opt
                          => opt.MapFrom(src => src.GrowthTemplate));

        // Маппинг навыков
        CreateMap<Skill, SkillDto>();
        CreateMap<SkillLevel, SkillLevelDto>()
           .ForMember(dest
                          => dest.Description, opt
                          => opt.Ignore()); // Заполним в сервисе

        // Маппинг талантов
        CreateMap<Talent, TalentDto>();
        CreateMap<TalentLevel, TalentLevelDto>()
           .ForMember(dest
                          => dest.Description, opt
                          => opt.Ignore()); // Заполним в сервисе

        // Маппинг базовых характеристик
        CreateMap<OperatorGrowthTemplate, OperatorBaseStatsDto>()
           .ForMember(dest
                          => dest.MaxE0Level, opt
                          => opt.MapFrom(src => GetMaxLevel(src.Operator.Rarity, 0)))
           .ForMember(dest
                          => dest.MaxE1Level, opt
                          => opt.MapFrom(src => GetMaxLevel(src.Operator.Rarity, 1)))
           .ForMember(dest
                          => dest.MaxE2Level, opt
                          => opt.MapFrom(src => GetMaxLevel(src.Operator.Rarity, 2)));

        // Маппинг рассчитанных характеристик
        CreateMap<OperatorStatsCalculator.OperatorCalculatedStats, OperatorStatsDto>();

        // Маппинг справочников
        CreateMap<OperatorClass, OperatorClassDto>();
        CreateMap<SubClass, SubClassDto>()
           .ForMember(dest
                          => dest.OperatorClassName, opt
                          => opt.MapFrom(src => src.OperatorClass.Name));
        CreateMap<Faction, FactionDto>();

        // Обратный маппинг для создания оператора
        CreateMap<OperatorCreateDto, Operator>()
           .ForMember(dest
                          => dest.Id, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.OperatorClass, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.SubClass, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.Faction, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.Skills, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.Talents, opt
                          => opt.Ignore())
           .ForMember(dest
                          => dest.OperatorModules, opt
                          => opt.Ignore());
        
        CreateMap<User, UserDto>()
           .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

    }

    private int? GetMaxLevel(int rarity, int elite)
    {
        var caps = OperatorStatsCalculator.GetLevelCaps(rarity);
        return elite switch
        {
            0 => caps.e0,
            1 => caps.e1,
            2 => caps.e2,
            _ => null
        };
    }
}