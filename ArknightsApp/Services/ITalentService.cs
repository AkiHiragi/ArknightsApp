using ArknightsApp.DTO;

namespace ArknightsApp.Services;

public interface ITalentService
{
    Task<List<TalentDto>> GetTalentsByOperatorIdAsync(int   operatorId);
    Task<TalentDto?>      GetTalentByIdAsync(int            id);
    Task<TalentDto>       CreateTalentAsync(TalentCreateDto talentDto);
    Task<TalentDto?>      UpdateTalentAsync(int             id, TalentCreateDto talentDto);
    Task<bool>            DeleteTalentAsync(int             id);
}