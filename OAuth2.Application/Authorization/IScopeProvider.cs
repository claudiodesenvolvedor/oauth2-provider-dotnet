namespace OAuth2.Application.Authorization;

public interface IScopeProvider
{
    Task<IReadOnlyList<Scope>> GetAllAsync(CancellationToken cancellationToken);
    Task<Scope?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
