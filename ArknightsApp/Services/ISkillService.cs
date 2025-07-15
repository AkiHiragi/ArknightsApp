using ArknightsApp.DTO;

namespace ArknightsApp.Services;

public interface ISkillService
{
    Task<List<SkillDto>> GetSkillsByOperatorIdAsync(int  operatorId);
    Task<SkillDto?>      GetSkillByIdAsync(int           id);
    Task<SkillDto>       CreateSkillAsync(SkillCreateDto skillDto);
    Task<SkillDto?>      UpdateSkillAsync(int            id, SkillCreateDto skillDto);
    Task<bool>           DeleteSkillAsync(int            id);
}