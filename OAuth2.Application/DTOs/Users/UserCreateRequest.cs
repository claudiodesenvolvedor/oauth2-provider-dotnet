namespace OAuth2.Application.DTOs.Users;

public sealed record UserCreateRequest(
    string Username,
    string Email,
    string Password,
    IReadOnlyList<string> Roles);
