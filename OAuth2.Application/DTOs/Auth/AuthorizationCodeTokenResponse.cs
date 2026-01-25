namespace OAuth2.Application.DTOs.Auth;

public sealed record AuthorizationCodeTokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? Scope);
