using ArknightsApp.ModelDto;
using FluentValidation;

namespace ArknightsApp.Validators;

public class OperatorDtoValidator : AbstractValidator<OperatorDto>
{
    public OperatorDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя оператора обязательно")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

        RuleFor(x => x.Rarity)
            .InclusiveBetween(1, 6).WithMessage("Редкость должна быть от 1 до 6");

        RuleFor(x => x.Class)
            .NotEmpty().WithMessage("Класс оператора обязателен")
            .MaximumLength(50);

        RuleFor(x => x.Subclass)
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}