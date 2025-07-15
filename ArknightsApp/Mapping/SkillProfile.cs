using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;

namespace ArknightsApp.Mapping;

public class SkillProfile:Profile
{
    public SkillProfile()
    {
        CreateMap<Skill, SkillDto>()
           .ForMember(dest 
                          => dest.Description, opt 
                          => opt.MapFrom(src => src.BaseDescription))
           .ForMember(dest 
                          => dest.Levels,      opt 
                          => opt.MapFrom(src => src.SkillLevels));

        CreateMap<SkillLevel, SkillLevelDto>()
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => GenerateDescription(src)));
    }
    
    private string GenerateDescription(SkillLevel skillLevel)
    {
        var description = skillLevel.Skill.BaseDescription;
        
        if (skillLevel.Parameters != null)
        {
            foreach (var param in skillLevel.Parameters)
            {
                description = description.Replace($"{{{param.Key}}}", param.Value.ToString());
            }
        }
        
        // Подставляем стандартные параметры
        description = description.Replace("{spCost}",   skillLevel.SpCost.ToString());
        description = description.Replace("{duration}", skillLevel.Duration.ToString());
        
        return description;
    }
}