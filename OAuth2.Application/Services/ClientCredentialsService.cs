using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Application.Services;

public sealed class ClientCredentialsService : IClientCredentialsService
{
    private readonly IClientStore _clientStore;
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenLifetimeProvider _tokenLifetimeProvider;

    public ClientCredentialsService(
        IClientStore clientStore,
        ITokenService tokenService,
        IAccessTokenLifetimeProvider tokenLifetimeProvider)
    {
        _clientStore = clientStore;
        _tokenService = tokenService;
        _tokenLifetimeProvider = tokenLifetimeProvider;
    }

    public Task<ClientCredentialsTokenResponse> IssueTokenAsync(
        ClientCredentialsTokenRequest request,
        CancellationToken cancellationToken)
    {
        if (!string.Equals(request.GrantType, "client_credentials", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid grant_type.");
        }

        return IssueTokenInternalAsync(request, cancellationToken);
    }

    private async Task<ClientCredentialsTokenResponse> IssueTokenInternalAsync(
        ClientCredentialsTokenRequest request,
        CancellationToken cancellationToken)
    {
        var client = await _clientStore.GetByClientIdAsync(request.ClientId, cancellationToken);
        if (client is null)
        {
            throw new InvalidOperationException("Invalid client_id.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.ClientSecret, client.ClientSecretHash))
        {
            throw new InvalidOperationException("Invalid client_secret.");
        }

        var requestedScopes = ResolveScopes(request.Scope);
        var allowedScopes = client.Scopes;
        if (requestedScopes.Count > 0 &&
            requestedScopes.Any(scope => !allowedScopes.Contains(scope)))
        {
            throw new InvalidOperationException("Invalid scope.");
        }

        var effectiveScopes = requestedScopes.Count > 0 ? requestedScopes : allowedScopes;
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes());
        var token = _tokenService.GenerateAccessToken(client.ClientId, effectiveScopes, expiresAt);

        return new ClientCredentialsTokenResponse(
            token,
            "Bearer",
            (int)TimeSpan.FromMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes()).TotalSeconds,
            effectiveScopes.Count > 0 ? string.Join(' ', effectiveScopes) : null);
    }

    private static IReadOnlyList<string> ResolveScopes(string? scope)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            return Array.Empty<string>();
        }

        return scope
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
