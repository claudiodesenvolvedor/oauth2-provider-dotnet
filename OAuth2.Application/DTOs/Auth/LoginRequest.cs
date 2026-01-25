namespace OAuth2.Application.DTOs.Auth;

public sealed record LoginRequest(
    string UsernameOrEmail,
    string Password);
