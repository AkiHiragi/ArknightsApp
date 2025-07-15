using System.Globalization;
using ArknightsApp.DTO;
using ArknightsApp.Models;

namespace ArknightsApp.Services;

public class SkillTemplateService : ISkillTemplateService
{
    public List<SkillLevelCreateDto> GenerateSkillLevels(SkillTemplate template)
    {
        var levels = new List<SkillLevelCreateDto>();

        for (var level = 1; level <= 10; level++)
        {
            var description = template.BaseDescription;
            var parameters  = new Dictionary<string, object>();

            foreach (var param in template.Parameters)
            {
                var value = Convert.ToDouble(param.Value.Values[level - 1]);
                description = description.Replace($"{{{param.Key}}}", value.ToString(CultureInfo.InvariantCulture));

                if (param.Key != "spCost" && param.Key != "duration")
                {
                    parameters[param.Key] = value;
                }
            }
            
            var spCost = template.Parameters.TryGetValue("spCost", out var sp) 
                             ? Convert.ToInt32(sp.Values[level - 1])
                             : 0;
    
            var duration = template.Parameters.TryGetValue("duration", out var dur) 
                               ? Convert.ToInt32(dur.Values[level - 1])
                               : 0;

            levels.Add(new SkillLevelCreateDto
            {
                Level      = level,
                LevelName  = $"Уровень {level}",
                SpCost     = spCost,
                Duration   = duration,
                Parameters = parameters
            });
        }

        return levels;
    }
}