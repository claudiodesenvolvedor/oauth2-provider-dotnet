namespace OAuth2.Application.Authorization;

public sealed record Scope(
    string Name,
    string? Description);
