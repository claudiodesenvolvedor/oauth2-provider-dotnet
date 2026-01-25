namespace OAuth2.Infrastructure.Persistence.Documents;

public sealed class ClientDocument
{
    public string Id { get; init; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecretHash { get; set; } = string.Empty;
    public IReadOnlyList<string> AllowedScopes { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> RedirectUris { get; set; } = Array.Empty<string>();
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
