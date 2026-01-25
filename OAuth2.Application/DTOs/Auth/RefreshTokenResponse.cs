namespace OAuth2.Application.DTOs.Auth;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? Scope,
    string RefreshToken);
