namespace OAuth2.Infrastructure.Settings;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    // RS256 keys will be provided by external strategy
    // (environment variables / secret manager / JWKS) in a later step.
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 10;
    public string KeyId { get; init; } = string.Empty;
}
