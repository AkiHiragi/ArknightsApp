using ArknightsApp.DTO;
using FluentValidation;

namespace ArknightsApp.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
           .NotEmpty().WithMessage("Имя пользователя обязательно")
           .Length(3, 50).WithMessage("Имя пользователя должно быть от 3 до 50 символов");

        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email обязателен")
           .EmailAddress().WithMessage("Некорректный формат email");

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Пароль обязателен")
           .MinimumLength(6).WithMessage("Пароль должен быть минимум 6 символов");

        RuleFor(x => x.ConfirmPassword)
           .Equal(x => x.Password).WithMessage("Пароли не совпадают");
    }
}