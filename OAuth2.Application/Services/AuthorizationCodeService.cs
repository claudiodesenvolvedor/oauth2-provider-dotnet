using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Application.Services;

public sealed class AuthorizationCodeService : IAuthorizationCodeService
{
    public Task<AuthorizationCodeResponse> CreateCodeAsync(
        AuthorizationCodeRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorizationCodeTokenResponse> ExchangeTokenAsync(
        AuthorizationCodeTokenRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
