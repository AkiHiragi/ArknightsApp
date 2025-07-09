using ArknightsApp.DTO;
using ArknightsApp.Services;
using FluentValidation;

namespace ArknightsApp.Validators;

// Базовый валидатор для регистрации в DI
public class StatRequestValidator : AbstractValidator<StatsRequestDto>
{
    public StatRequestValidator()
    {
        RuleFor(x => x.Level)
           .GreaterThan(0).WithMessage("Уровень должен быть больше 0")
           .LessThanOrEqualTo(90).WithMessage("Максимальный уровень - 90");

        RuleFor(x => x.Elite)
           .InclusiveBetween(0, 2).WithMessage("Элита должна быть 0, 1 или 2");

        RuleFor(x => x.TrustLevel)
           .InclusiveBetween(0, 200).WithMessage("Траст должен быть от 0 до 200");
    }
}

// Валидатор с учетом редкости - создается вручную в сервисе
public class StatRequestWithRarityValidator : AbstractValidator<StatsRequestDto>
{
    private readonly int _operatorRarity;

    public StatRequestWithRarityValidator(int operatorRarity)
    {
        _operatorRarity = operatorRarity;
        
        RuleFor(x => x.Level)
           .GreaterThan(0).WithMessage("Уровень должен быть больше 0");

        RuleFor(x => x.Elite)
           .InclusiveBetween(0, 2).WithMessage("Элита должна быть 0, 1 или 2")
           .Must(BeValidEliteForRarity)
           .WithMessage($"Элита {{PropertyValue}} недоступна для {_operatorRarity}★ оператора");

        RuleFor(x => x.TrustLevel)
           .InclusiveBetween(0, 200).WithMessage("Траст должен быть от 0 до 200");

        // Комплексная валидация уровня с учетом элиты и редкости
        RuleFor(x => x)
           .Must(BeValidLevelForEliteAndRarity)
           .WithMessage(x =>
            {
                var maxLevel = GetMaxLevelForElite(x.Elite);
                return
                    $"Уровень {x.Level} недоступен для E{x.Elite} {_operatorRarity}★ оператора (макс: {maxLevel})";
            });
    }

    private bool BeValidEliteForRarity(int elite)
    {
        return OperatorStatsCalculator.CanPromoteToElite(_operatorRarity, elite);
    }

    private bool BeValidLevelForEliteAndRarity(StatsRequestDto request)
    {
        if (!BeValidEliteForRarity(request.Elite))
            return false;

        var maxLevel = GetMaxLevelForElite(request.Elite);
        return maxLevel.HasValue && request.Level <= maxLevel;
    }

    private int? GetMaxLevelForElite(int elite)
    {
        var caps = OperatorStatsCalculator.GetLevelCaps(_operatorRarity);
        return elite switch
        {
            0 => caps.e0,
            1 => caps.e1,
            2 => caps.e2,
            _ => null
        };
    }
}
