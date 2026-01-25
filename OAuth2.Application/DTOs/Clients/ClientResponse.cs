namespace OAuth2.Application.DTOs.Clients;

public sealed record ClientResponse(
    string ClientId,
    IReadOnlyList<string> Scopes,
    bool IsActive,
    DateTimeOffset CreatedAt);
