namespace OAuth2.Application.Authorization;

public interface IAuthorizationPolicyProvider
{
    Task<IReadOnlyList<Policy>> GetPoliciesAsync(CancellationToken cancellationToken);
    Task<Policy?> GetPolicyAsync(string name, CancellationToken cancellationToken);
}
