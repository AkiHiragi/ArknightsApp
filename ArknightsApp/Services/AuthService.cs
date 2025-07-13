using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArknightsApp.Validators;

namespace ArknightsApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ArknightsDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ArknightsDbContext context,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation("Попытка входа пользователя: {Username}", request.Username);

            // Находим пользователя
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Неверный логин или пароль");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Аккаунт заблокирован");
            }

            // Обновляем время последнего входа
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Генерируем токен
            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto,
                ExpiresAt = DateTime.UtcNow.AddHours(24) // Токен действует 24 часа
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var validator = new RegisterRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }
            
            _logger.LogInformation("Регистрация нового пользователя: {Username}", request.Username);

            // Проверяем, что пользователь не существует
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            }

            // Создаем нового пользователя
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.User, // По умолчанию обычный пользователь
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Генерируем токен
            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long!";
            var issuer = jwtSettings["Issuer"] ?? "ArknightsApp";
            var audience = jwtSettings["Audience"] ?? "ArknightsApp";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
