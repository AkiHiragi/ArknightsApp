using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Services;

public class SkillService : ISkillService
{
    private readonly ArknightsDbContext    _context;
    private readonly IMapper               _mapper;
    private readonly ILogger<SkillService> _logger;

    public SkillService(ArknightsDbContext context, IMapper mapper, ILogger<SkillService> logger)
    {
        _context = context;
        _mapper  = mapper;
        _logger  = logger;
    }

    public async Task<List<SkillDto>> GetSkillsByOperatorIdAsync(int operatorId)
    {
        _logger.LogInformation("Получение скиллов оператора {OperatorId}", operatorId);

        var skills = await _context.Skills
                                   .Include(s => s.SkillLevels)
                                   .Where(s => s.OperatorId == operatorId)
                                   .OrderBy(s => s.Id)
                                   .ToListAsync();

        return _mapper.Map<List<SkillDto>>(skills);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        _logger.LogInformation("Получение скилла {SkillId}", id);

        var skill = await _context.Skills
                                  .Include(s => s.SkillLevels)
                                  .FirstOrDefaultAsync(s => s.Id == id);

        return skill == null ? null : _mapper.Map<SkillDto>(skill);
    }

    public async Task<SkillDto> CreateSkillAsync(SkillCreateDto skillDto)
    {
        _logger.LogInformation("Создание нового скилла {SkillName} для оператора {OperatorId}",
                               skillDto.Name, skillDto.OperatorId);

        var skill = new Skill
        {
            Name            = skillDto.Name,
            BaseDescription = skillDto.BaseDescription,
            IconUrl         = skillDto.IconUrl,
            OperatorId      = skillDto.OperatorId
        };

        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        
        var skillLevels = skillDto.Levels.Select(l => new SkillLevel
        {
            SkillId    = skill.Id,
            Level      = l.Level,
            LevelName  = l.LevelName,
            SpCost     = l.SpCost,
            Duration   = l.Duration,
            Parameters = l.Parameters
        }).ToList();

        _context.SkillLevels.AddRange(skillLevels);
        await _context.SaveChangesAsync();

        return await GetSkillByIdAsync(skill.Id) ?? throw new InvalidOperationException("Не удалось создать скилл");
    }

    public async Task<SkillDto?> UpdateSkillAsync(int id, SkillCreateDto skillDto)
    {
        _logger.LogInformation("Обновление скилла {SkillId}", id);

        var skill = await _context.Skills
                                  .Include(s => s.SkillLevels)
                                  .FirstOrDefaultAsync(s => s.Id == id);

        if (skill == null)
            return null;

        skill.Name            = skillDto.Name;
        skill.BaseDescription = skillDto.BaseDescription;
        skill.IconUrl         = skillDto.IconUrl;
        
        _context.SkillLevels.RemoveRange(skill.SkillLevels);
        await _context.SaveChangesAsync();
        
        var newSkillLevels = skillDto.Levels.Select(l => new SkillLevel
        {
            SkillId    = skill.Id,
            Level      = l.Level,
            LevelName  = l.LevelName,
            SpCost     = l.SpCost,
            Duration   = l.Duration,
            Parameters = l.Parameters
        }).ToList();

        _context.SkillLevels.AddRange(newSkillLevels);
        await _context.SaveChangesAsync();

        return await GetSkillByIdAsync(skill.Id);
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        _logger.LogInformation("Удаление скилла {SkillId}", id);

        var skill = await _context.Skills
                                  .Include(s => s.SkillLevels)
                                  .FirstOrDefaultAsync(s => s.Id == id);

        if (skill == null)
            return false;

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();

        return true;
    }
}