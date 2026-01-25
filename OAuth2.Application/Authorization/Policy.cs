namespace OAuth2.Application.Authorization;

public sealed record Policy(
    string Name,
    IReadOnlyList<string> RequiredScopes);
