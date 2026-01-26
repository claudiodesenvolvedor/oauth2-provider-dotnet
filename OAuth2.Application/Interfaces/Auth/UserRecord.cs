namespace OAuth2.Application.Interfaces.Auth;

public sealed record UserRecord(
    string Id,
    string Email,
    string PasswordHash,
    bool IsActive,
    DateTimeOffset CreatedAt);
