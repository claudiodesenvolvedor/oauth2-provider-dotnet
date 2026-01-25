namespace OAuth2.Application.DTOs.Auth;

public sealed record ClientCredentialsTokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string? Scope);
