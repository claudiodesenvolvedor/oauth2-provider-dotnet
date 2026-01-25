namespace OAuth2.Infrastructure.Persistence.Documents;

public sealed class TokenDocument
{
    public string Id { get; init; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public IReadOnlyList<string> Scopes { get; set; } = Array.Empty<string>();
    public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow;
}
