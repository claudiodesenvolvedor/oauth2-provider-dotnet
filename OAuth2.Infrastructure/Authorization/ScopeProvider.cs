using OAuth2.Application.Authorization;

namespace OAuth2.Infrastructure.Authorization;

public sealed class ScopeProvider : IScopeProvider
{
    public Task<IReadOnlyList<Scope>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Scope?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
