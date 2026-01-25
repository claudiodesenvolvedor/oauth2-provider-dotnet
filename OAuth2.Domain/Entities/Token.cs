namespace OAuth2.Domain.Entities;

public sealed class Token
{
    public string Id { get; init; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public IReadOnlyList<string> Scopes { get; set; } = Array.Empty<string>();
    public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow;
}
