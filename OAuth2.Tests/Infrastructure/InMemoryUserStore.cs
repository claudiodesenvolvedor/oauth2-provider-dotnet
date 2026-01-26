using System.Collections.Concurrent;
using OAuth2.Application.Interfaces.Auth;

namespace OAuth2.Tests.Infrastructure;

public sealed class InMemoryUserStore : IUserStore
{
    private readonly ConcurrentDictionary<string, UserRecord> _users = new();

    public Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(!_users.IsEmpty);
    }

    public Task<UserRecord?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult<UserRecord?>(null);
        }

        _users.TryGetValue(email, out var user);
        return Task.FromResult<UserRecord?>(user);
    }

    public Task<UserRecord> CreateAsync(string email, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Password is required.");
        }

        var user = new UserRecord(
            Guid.NewGuid().ToString("N"),
            email,
            BCrypt.Net.BCrypt.HashPassword(password),
            true,
            DateTimeOffset.UtcNow);

        _users[email] = user;
        return Task.FromResult(user);
    }
}
