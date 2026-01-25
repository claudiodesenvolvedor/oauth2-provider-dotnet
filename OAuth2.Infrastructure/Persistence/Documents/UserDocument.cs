namespace OAuth2.Infrastructure.Persistence.Documents;

public sealed class UserDocument
{
    public string Id { get; init; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
