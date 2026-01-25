namespace OAuth2.Application.DTOs.Auth;

public sealed record AuthorizationCodeResponse(
    string Code,
    string RedirectUri,
    string? State);
