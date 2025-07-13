using ArknightsApp.DTO;
using ArknightsApp.Models;

namespace ArknightsApp.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto       request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<User?>           GetUserByUsernameAsync(string    username);
}