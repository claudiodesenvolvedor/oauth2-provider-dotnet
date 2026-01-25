using OAuth2.Application.Authorization;

namespace OAuth2.Infrastructure.Authorization;

public sealed class AuthorizationCodeStore : IAuthorizationCodeStore
{
    public Task StoreAsync(AuthorizationCode code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorizationCode?> GetAsync(string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateAsync(string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
