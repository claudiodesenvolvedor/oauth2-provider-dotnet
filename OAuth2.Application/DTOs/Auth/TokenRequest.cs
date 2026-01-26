namespace OAuth2.Application.DTOs.Auth;

public sealed record TokenRequest(
    string? GrantType,
    string? ClientId,
    string? ClientSecret,
    string? Scope,
    string? Code,
    string? RedirectUri,
    string? RefreshToken,
    string? CodeVerifier);
