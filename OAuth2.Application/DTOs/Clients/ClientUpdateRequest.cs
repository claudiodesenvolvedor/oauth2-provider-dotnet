namespace OAuth2.Application.DTOs.Clients;

public sealed record ClientUpdateRequest(
    IReadOnlyList<string> Scopes,
    bool IsActive);
