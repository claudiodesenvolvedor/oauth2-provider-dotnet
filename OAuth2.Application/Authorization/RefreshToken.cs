namespace OAuth2.Application.Authorization;

public sealed record RefreshToken(
    string Token,
    string ClientId,
    string Subject,
    IReadOnlyList<string> Scopes,
    DateTimeOffset ExpiresAt);
