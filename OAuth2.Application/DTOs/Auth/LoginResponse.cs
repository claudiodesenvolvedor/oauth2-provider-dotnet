namespace OAuth2.Application.DTOs.Auth;

public sealed record LoginResponse(
    string AccessToken,
    int ExpiresIn,
    string TokenType);
