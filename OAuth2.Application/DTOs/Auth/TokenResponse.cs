namespace OAuth2.Application.DTOs.Auth;

public sealed record TokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? Scope,
    string? RefreshToken);
