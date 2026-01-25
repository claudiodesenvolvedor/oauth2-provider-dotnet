using OAuth2.Application.DTOs.Auth;

namespace OAuth2.Application.Interfaces.Services;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(
        string clientId,
        string subject,
        IReadOnlyList<string> scopes,
        CancellationToken cancellationToken);

    Task<RefreshTokenResponse> ExchangeAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken);
}
