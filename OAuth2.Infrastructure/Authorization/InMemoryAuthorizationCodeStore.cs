using System.Collections.Concurrent;
using OAuth2.Application.Authorization;

namespace OAuth2.Infrastructure.Authorization;

public sealed class InMemoryAuthorizationCodeStore : IAuthorizationCodeStore
{
    private readonly ConcurrentDictionary<string, AuthorizationCode> _codes = new();

    public Task StoreAsync(AuthorizationCode code, CancellationToken cancellationToken)
    {
        _codes[code.Code] = code;
        return Task.CompletedTask;
    }

    public Task<AuthorizationCode?> GetAsync(string code, CancellationToken cancellationToken)
    {
        _codes.TryGetValue(code, out var result);
        return Task.FromResult(result);
    }

    public Task InvalidateAsync(string code, CancellationToken cancellationToken)
    {
        _codes.TryRemove(code, out _);
        return Task.CompletedTask;
    }
}
