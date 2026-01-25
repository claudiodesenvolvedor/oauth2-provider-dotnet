namespace OAuth2.Application.Interfaces.Auth;

public sealed record ClientRecord(
    string ClientId,
    string ClientSecretHash,
    IReadOnlyList<string> Scopes);
