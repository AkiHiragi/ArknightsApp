using ArknightsApp.DTO;
using FluentValidation;

namespace ArknightsApp.Validators;

public class OperatorCreateValidator : AbstractValidator<OperatorCreateDto>
{
    public OperatorCreateValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Имя оператора обязательно")
           .Length(1, 50).WithMessage("Имя должно быть в пределах 1-50 символов")
           .Matches(@"^[a-zA-Z0-9\s\-'\.ё]+$").WithMessage("Имя содержит недопустимые символы");

        RuleFor(x => x.Rarity)
           .InclusiveBetween(1, 6).WithMessage("Редкость должна быть в промежутке от 1 до 6 звезд");

        RuleFor(x => x.OperatorClassId)
           .GreaterThan(0).WithMessage("Класс оператора обязателен");

        RuleFor(x => x.SubClassId)
           .GreaterThan(0).WithMessage("Подкласс оператора обязателен");

        RuleFor(x => x.Position)
           .Must(x => x is "Melee" or "Ranged")
           .WithMessage("Позиция должна быть 'Melee' или 'Ranged'");

        RuleFor(x => x.ImageUrl)
           .NotEmpty().WithMessage("URL изображения обязателен")
           .Must(BeValidUrl).WithMessage("Некорректный URL изображения");

        RuleFor(x => x.Description)
           .MaximumLength(1000).WithMessage("Описание не должно превышать 1000 символов");

        RuleFor(x => x.CnReleaseDate)
           .NotEmpty().WithMessage("Дата релиза в CN обязательна")
           .LessThanOrEqualTo(DateTime.Now + TimeSpan.FromDays(10))
           .WithMessage("Дата релиза в CN не может быть в будущем");

        RuleFor(x => x.GlobalReleaseDate)
           .GreaterThanOrEqualTo(x => x.CnReleaseDate)
           .WithMessage("Дата глобального релиза не может быть раньше CN релиза")
           .LessThanOrEqualTo(DateTime.Now.AddYears(2))
           .WithMessage("Дата глобального релиза слишком далеко в будущем")
           .When(x => x.GlobalReleaseDate.HasValue);
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}