namespace OAuth2.Infrastructure.Persistence.Documents;

public sealed class RefreshTokenDocument
{
    public string Token { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public IReadOnlyList<string> Scopes { get; set; } = Array.Empty<string>();
    public DateTimeOffset ExpiresAt { get; set; }
}
