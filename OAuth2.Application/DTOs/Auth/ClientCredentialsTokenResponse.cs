namespace OAuth2.Application.DTOs.Auth;

public sealed record ClientCredentialsTokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? Scope);
