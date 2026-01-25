namespace OAuth2.Application.DTOs.Users;

public sealed record UserResponse(
    string Id,
    string Username,
    string Email,
    IReadOnlyList<string> Roles,
    DateTimeOffset CreatedAt);
