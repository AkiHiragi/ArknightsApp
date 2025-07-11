namespace ArknightsApp.Models;

public class User
{
    public          int       Id           { get; set; }
    public required string    Username     { get; set; }
    public required string    Email        { get; set; }
    public required string    PasswordHash { get; set; }
    public          UserRole  Role         { get; set; }
    public          DateTime  CreatedAt    { get; set; }
    public          DateTime? LastLoginAt  { get; set; }
    public          bool      IsActive     { get; set; }
}

public enum UserRole
{
    User=0,
    Moderator=1,
    Admin=2
}