namespace OAuth2.Infrastructure.Persistence.Documents;

public sealed class AuthorizationCodeDocument
{
    public string Code { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? RedirectUri { get; set; }
    public IReadOnlyList<string> Scopes { get; set; } = Array.Empty<string>();
    public DateTimeOffset ExpiresAt { get; set; }
}
