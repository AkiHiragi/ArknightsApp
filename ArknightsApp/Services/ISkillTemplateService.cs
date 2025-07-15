using ArknightsApp.DTO;
using ArknightsApp.Models;

namespace ArknightsApp.Services;

public interface ISkillTemplateService
{
    List<SkillLevelCreateDto> GenerateSkillLevels(SkillTemplate template);
}