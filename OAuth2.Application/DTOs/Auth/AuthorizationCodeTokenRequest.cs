namespace OAuth2.Application.DTOs.Auth;

public sealed record AuthorizationCodeTokenRequest(
    string ClientId,
    string ClientSecret,
    string Code,
    string RedirectUri);
