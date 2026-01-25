namespace OAuth2.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(
        string subject,
        IReadOnlyList<string> scopes,
        DateTimeOffset expiresAt);
}
