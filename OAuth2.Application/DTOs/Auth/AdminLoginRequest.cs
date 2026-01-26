namespace OAuth2.Application.DTOs.Auth;

public sealed record AdminLoginRequest(
    string Email,
    string Password);
