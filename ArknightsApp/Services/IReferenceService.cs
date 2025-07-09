using ArknightsApp.DTO;

namespace ArknightsApp.Services;

public interface IReferenceService
{
    Task<List<OperatorClassDto>> GetOperatorClassesAsync();
    Task<List<SubClassDto>>      GetSubClassesByClassIdAsync(int classId);
    Task<List<FactionDto>>       GetFactionsAsync();
    Task<OperatorCreateFormDto>  GetOperatorCreateFormDataAsync();
}