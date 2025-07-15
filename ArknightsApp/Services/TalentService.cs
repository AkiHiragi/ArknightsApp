using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Services
{
    public class TalentService : ITalentService
    {
        private readonly ArknightsDbContext     _context;
        private readonly IMapper                _mapper;
        private readonly ILogger<TalentService> _logger;

        public TalentService(ArknightsDbContext context, IMapper mapper, ILogger<TalentService> logger)
        {
            _context = context;
            _mapper  = mapper;
            _logger  = logger;
        }

        public async Task<List<TalentDto>> GetTalentsByOperatorIdAsync(int operatorId)
        {
            _logger.LogInformation("Получение талантов для оператора {OperatorId}", operatorId);

            var talents = await _context.Talents
                                        .Include(t => t.TalentLevels)
                                        .Where(t => t.OperatorId == operatorId)
                                        .OrderBy(t => t.Id)
                                        .ToListAsync();

            return _mapper.Map<List<TalentDto>>(talents);
        }

        public async Task<TalentDto?> GetTalentByIdAsync(int id)
        {
            _logger.LogInformation("Получение таланта {TalentId}", id);

            var talent = await _context.Talents
                                       .Include(t => t.TalentLevels)
                                       .FirstOrDefaultAsync(t => t.Id == id);

            return talent == null ? null : _mapper.Map<TalentDto>(talent);
        }

        public async Task<TalentDto> CreateTalentAsync(TalentCreateDto talentDto)
        {
            _logger.LogInformation("Создание нового таланта {TalentName} для оператора {OperatorId}",
                                   talentDto.Name, talentDto.OperatorId);

            var talent = new Talent
            {
                Name            = talentDto.Name,
                BaseDescription = talentDto.BaseDescription,
                IconUrl         = talentDto.IconUrl,
                OperatorId      = talentDto.OperatorId
            };

            _context.Talents.Add(talent);
            await _context.SaveChangesAsync();
            
            var talentLevels = talentDto.Levels.Select(l => new TalentLevel
            {
                TalentId        = talent.Id,
                PotentialLevel  = l.PotentialLevel,
                UnlockCondition = l.UnlockCondition,
                Parameters      = l.Parameters
            }).ToList();

            _context.TalentLevels.AddRange(talentLevels);
            await _context.SaveChangesAsync();

            return await GetTalentByIdAsync(talent.Id) ??
                   throw new InvalidOperationException("Не удалось создать талант");
        }

        public async Task<TalentDto?> UpdateTalentAsync(int id, TalentCreateDto talentDto)
        {
            _logger.LogInformation("Обновление таланта {TalentId}", id);

            var talent = await _context.Talents
                                       .Include(t => t.TalentLevels)
                                       .FirstOrDefaultAsync(t => t.Id == id);

            if (talent == null)
                return null;

            talent.Name            = talentDto.Name;
            talent.BaseDescription = talentDto.BaseDescription;
            talent.IconUrl         = talentDto.IconUrl;
            
            _context.TalentLevels.RemoveRange(talent.TalentLevels);
            await _context.SaveChangesAsync();
            
            var newTalentLevels = talentDto.Levels.Select(l => new TalentLevel
            {
                TalentId        = talent.Id,
                PotentialLevel  = l.PotentialLevel,
                UnlockCondition = l.UnlockCondition,
                Parameters      = l.Parameters
            }).ToList();

            _context.TalentLevels.AddRange(newTalentLevels);
            await _context.SaveChangesAsync();

            return await GetTalentByIdAsync(talent.Id);
        }

        public async Task<bool> DeleteTalentAsync(int id)
        {
            _logger.LogInformation("Удаление таланта {TalentId}", id);

            var talent = await _context.Talents
                                       .Include(t => t.TalentLevels)
                                       .FirstOrDefaultAsync(t => t.Id == id);

            if (talent == null)
                return false;

            _context.Talents.Remove(talent);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}