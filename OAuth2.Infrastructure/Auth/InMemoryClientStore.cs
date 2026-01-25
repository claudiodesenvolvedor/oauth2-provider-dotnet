using OAuth2.Application.Interfaces.Auth;

namespace OAuth2.Infrastructure.Auth;

public sealed class InMemoryClientStore : IClientStore
{
    private const string TestClientSecretHash =
        "$2a$11$jhPtzl1UY6MjrRdRt58HuuFw8IykUHWF69ky6o0Eb6Kgu6CbqECPC";

    private static readonly ClientRecord Client = new(
        "test-client",
        TestClientSecretHash,
        new[] { "api.read" });

    public Task<ClientRecord?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        if (string.Equals(clientId, Client.ClientId, StringComparison.Ordinal))
            return Task.FromResult<ClientRecord?>(Client);

        return Task.FromResult<ClientRecord?>(null);
    }
}
