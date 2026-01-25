using System.Collections.Concurrent;
using OAuth2.Application.Authorization;

namespace OAuth2.Infrastructure.Authorization;

public sealed class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private readonly ConcurrentDictionary<string, RefreshToken> _tokens = new();

    public Task StoreAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        _tokens[token.Token] = token;
        return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken)
    {
        _tokens.TryGetValue(token, out var result);
        return Task.FromResult(result);
    }

    public Task InvalidateAsync(string token, CancellationToken cancellationToken)
    {
        _tokens.TryRemove(token, out _);
        return Task.CompletedTask;
    }
}
