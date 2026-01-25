using OAuth2.Application.DTOs.Auth;

namespace OAuth2.Application.Interfaces.Services;

public interface IAuthorizationCodeService
{
    Task<AuthorizationCodeResponse> CreateCodeAsync(
        AuthorizationCodeRequest request,
        CancellationToken cancellationToken);

    Task<AuthorizationCodeTokenResponse> ExchangeTokenAsync(
        AuthorizationCodeTokenRequest request,
        CancellationToken cancellationToken);
}
