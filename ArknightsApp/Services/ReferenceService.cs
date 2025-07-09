using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;

namespace ArknightsApp.Services;

public class ReferenceService : IReferenceService
{
    private readonly IMapper                   _mapper;
    private readonly ILogger<ReferenceService> _logger;

    private readonly List<OperatorClass> _operatorClasses;
    private readonly List<SubClass>      _subClasses;
    private readonly List<Faction>       _factions;


    public ReferenceService(IMapper mapper, ILogger<ReferenceService> logger)
    {
        _mapper          = mapper;
        _logger          = logger;
        _operatorClasses = GetSampleOperatorClasses();
        _subClasses      = GetSampleSubClasses();
        _factions        = GetSampleFactions();
    }

    public async Task<List<OperatorClassDto>> GetOperatorClassesAsync()
    {
        _logger.LogInformation("Получение списка классов операторов");
        return await Task.FromResult(_mapper.Map<List<OperatorClassDto>>(_operatorClasses));
    }

    public async Task<List<SubClassDto>> GetSubClassesByClassIdAsync(int classId)
    {
        _logger.LogInformation("Получение подклассов для класса {ClassId}", classId);
        var subClasses = _subClasses.Where(x => x.OperatorClassId == classId).ToList();
        return await Task.FromResult(_mapper.Map<List<SubClassDto>>(subClasses));
    }

    public async Task<List<FactionDto>> GetFactionsAsync()
    {
        _logger.LogInformation("Получение списка фракций");
        return await Task.FromResult(_mapper.Map<List<FactionDto>>(_factions));
    }

    public async Task<OperatorCreateFormDto> GetOperatorCreateFormDataAsync()
    {
        _logger.LogInformation("Получение данных для формы создания оператора");

        var formData = new OperatorCreateFormDto
        {
            AvailableClasses   = await GetOperatorClassesAsync(),
            AvailableFactions  = await GetFactionsAsync(),
            AvailablePositions = ["Melee", "Ranged"],
            AvailableRarities  = [1, 2, 3, 4, 5, 6]
        };

        return formData;
    }

    private List<OperatorClass> GetSampleOperatorClasses()
    {
        return new List<OperatorClass>
        {
            new()
            {
                Id = 1, Name = "Guard", Description = "Ближний бой, блокирует врагов", IconUrl = "/icons/guard.png"
            },
            new()
            {
                Id = 2, Name = "Caster", Description = "Магический урон на расстоянии", IconUrl = "/icons/caster.png"
            },
            new()
            {
                Id = 3, Name = "Sniper", Description = "Физический урон на расстоянии", IconUrl = "/icons/sniper.png"
            },
            new()
            {
                Id      = 4, Name = "Defender", Description = "Высокая защита, блокирует врагов",
                IconUrl = "/icons/defender.png"
            },
            new() { Id = 5, Name = "Medic", Description     = "Лечит союзников", IconUrl     = "/icons/medic.png" },
            new() { Id = 6, Name = "Supporter", Description = "Поддержка и дебаффы", IconUrl = "/icons/supporter.png" },
            new()
            {
                Id = 7, Name = "Vanguard", Description = "Генерирует DP, первая линия", IconUrl = "/icons/vanguard.png"
            },
            new()
            {
                Id = 8, Name = "Specialist", Description = "Уникальные способности", IconUrl = "/icons/specialist.png"
            }
        };
    }

    private List<SubClass> GetSampleSubClasses()
    {
        return new List<SubClass>
        {
            // Guard подклассы
            new() { Id = 1, Name = "Arts Guard", OperatorClassId    = 1, OperatorClass = _operatorClasses[0] },
            new() { Id = 2, Name = "Duelist Guard", OperatorClassId = 1, OperatorClass = _operatorClasses[0] },
            new() { Id = 3, Name = "Lord Guard", OperatorClassId    = 1, OperatorClass = _operatorClasses[0] },

            // Caster подклассы
            new() { Id = 4, Name = "Core Caster", OperatorClassId   = 2, OperatorClass = _operatorClasses[1] },
            new() { Id = 5, Name = "Splash Caster", OperatorClassId = 2, OperatorClass = _operatorClasses[1] },
            new() { Id = 6, Name = "Chain Caster", OperatorClassId  = 2, OperatorClass = _operatorClasses[1] },

            // Sniper подклассы
            new() { Id = 7, Name = "Marksman Sniper", OperatorClassId = 3, OperatorClass = _operatorClasses[2] },
            new() { Id = 8, Name = "Spreadshooter", OperatorClassId   = 3, OperatorClass = _operatorClasses[2] },
            new() { Id = 9, Name = "Deadeye Sniper", OperatorClassId  = 3, OperatorClass = _operatorClasses[2] }
        };
    }

    private List<Faction> GetSampleFactions()
    {
        return new List<Faction>
        {
            new()
            {
                Id      = 1, Name = "Rhodes Island", Description = "Главная фармацевтическая компания",
                LogoUrl = "/logos/rhodes.png"
            },
            new() { Id = 2, Name = "Lungmen", Description = "Город-государство", LogoUrl = "/logos/lungmen.png" },
            new()
            {
                Id      = 3, Name = "Penguin Logistics", Description = "Логистическая компания",
                LogoUrl = "/logos/penguin.png"
            },
            new() { Id = 4, Name = "Karlan Trade", Description = "Торговая компания", LogoUrl = "/logos/karlan.png" },
            new() { Id = 5, Name = "Blacksteel", Description   = "Военная компания", LogoUrl = "/logos/blacksteel.png" }
        };
    }
}