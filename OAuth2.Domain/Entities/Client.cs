namespace OAuth2.Domain.Entities;

public sealed class Client
{
    public string Id { get; init; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecretHash { get; set; } = string.Empty;
    public IReadOnlyList<string> Scopes { get; set; } = Array.Empty<string>();
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
