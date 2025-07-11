namespace ArknightsApp.DTO;

public class LoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class RegisterRequestDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}

public class AuthResponseDto
{
    public required string   Token     { get; set; }
    public required UserDto  User      { get; set; }
    public          DateTime ExpiresAt { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}