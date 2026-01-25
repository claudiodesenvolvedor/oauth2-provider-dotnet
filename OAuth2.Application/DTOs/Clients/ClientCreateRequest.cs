namespace OAuth2.Application.DTOs.Clients;

public sealed record ClientCreateRequest(
    IReadOnlyList<string> Scopes);
