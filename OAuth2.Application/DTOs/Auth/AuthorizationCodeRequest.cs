namespace OAuth2.Application.DTOs.Auth;

public sealed record AuthorizationCodeRequest(
    string ClientId,
    string RedirectUri,
    string ResponseType,
    string? Scope,
    string? State);
