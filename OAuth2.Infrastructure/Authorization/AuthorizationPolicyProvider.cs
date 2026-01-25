using OAuth2.Application.Authorization;

namespace OAuth2.Infrastructure.Authorization;

public sealed class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<IReadOnlyList<Policy>> GetPoliciesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Policy?> GetPolicyAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
