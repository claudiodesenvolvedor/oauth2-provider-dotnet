namespace OAuth2.Application.Authorization;

public sealed record AuthorizationCode(
    string Code,
    string ClientId,
    string Subject,
    IReadOnlyList<string> Scopes,
    string? RedirectUri,
    DateTimeOffset ExpiresAt);
