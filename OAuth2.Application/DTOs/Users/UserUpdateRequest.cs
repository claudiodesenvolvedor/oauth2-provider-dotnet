namespace OAuth2.Application.DTOs.Users;

public sealed record UserUpdateRequest(
    string Username,
    string Email,
    IReadOnlyList<string> Roles);
