using OAuth2.Application.Authorization;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Application.Services;

public sealed class AuthorizationCodeService : IAuthorizationCodeService
{
    private const int AuthorizationCodeMinutes = 5;
    private readonly IClientStore _clientStore;
    private readonly IAuthorizationCodeStore _codeStore;
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenLifetimeProvider _tokenLifetimeProvider;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthorizationCodeService(
        IClientStore clientStore,
        IAuthorizationCodeStore codeStore,
        ITokenService tokenService,
        IAccessTokenLifetimeProvider tokenLifetimeProvider,
        IRefreshTokenService refreshTokenService)
    {
        _clientStore = clientStore;
        _codeStore = codeStore;
        _tokenService = tokenService;
        _tokenLifetimeProvider = tokenLifetimeProvider;
        _refreshTokenService = refreshTokenService;
    }

    public Task<AuthorizationCodeResponse> CreateCodeAsync(
        AuthorizationCodeRequest request,
        CancellationToken cancellationToken)
    {
        return CreateCodeInternalAsync(request, cancellationToken);
    }

    public Task<AuthorizationCodeTokenResponse> ExchangeTokenAsync(
        AuthorizationCodeTokenRequest request,
        CancellationToken cancellationToken)
    {
        return ExchangeTokenInternalAsync(request, cancellationToken);
    }

    private async Task<AuthorizationCodeResponse> CreateCodeInternalAsync(
        AuthorizationCodeRequest request,
        CancellationToken cancellationToken)
    {
        if (!string.Equals(request.ResponseType, "code", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid response_type.");
        }

        var client = await _clientStore.GetByClientIdAsync(request.ClientId, cancellationToken);
        if (client is null)
        {
            throw new InvalidOperationException("Invalid client_id.");
        }

        if (string.IsNullOrWhiteSpace(request.RedirectUri))
        {
            throw new InvalidOperationException("Invalid redirect_uri.");
        }

        var requestedScopes = ResolveScopes(request.Scope);
        if (requestedScopes.Count > 0 &&
            requestedScopes.Any(scope => !client.Scopes.Contains(scope)))
        {
            throw new InvalidOperationException("Invalid scope.");
        }

        var effectiveScopes = requestedScopes.Count > 0 ? requestedScopes : client.Scopes;
        var code = Guid.NewGuid().ToString("N");
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(AuthorizationCodeMinutes);

        var authorizationCode = new AuthorizationCode(
            code,
            client.ClientId,
            client.ClientId,
            effectiveScopes,
            request.RedirectUri,
            expiresAt);

        await _codeStore.StoreAsync(authorizationCode, cancellationToken);

        return new AuthorizationCodeResponse(
            code,
            request.RedirectUri,
            request.State);
    }

    private async Task<AuthorizationCodeTokenResponse> ExchangeTokenInternalAsync(
        AuthorizationCodeTokenRequest request,
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

        var code = await _codeStore.GetAsync(request.Code, cancellationToken);
        if (code is null)
        {
            throw new InvalidOperationException("Invalid code.");
        }

        if (code.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            await _codeStore.InvalidateAsync(request.Code, cancellationToken);
            throw new InvalidOperationException("Code expired.");
        }

        if (!string.Equals(code.ClientId, request.ClientId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid client.");
        }

        if (!string.IsNullOrWhiteSpace(code.RedirectUri) &&
        !string.Equals(code.RedirectUri, request.RedirectUri, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid redirect_uri.");
        }

        await _codeStore.InvalidateAsync(request.Code, cancellationToken);

        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes());
        var token = _tokenService.GenerateAccessToken(
            code.Subject,
            code.Scopes,
            expiresAt);

        var refreshToken = await _refreshTokenService.CreateAsync(
            code.ClientId,
            code.Subject,
            code.Scopes,
            cancellationToken);

        return new AuthorizationCodeTokenResponse(
            token,
            "Bearer",
            (int)TimeSpan.FromMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes()).TotalSeconds,
            code.Scopes.Count > 0 ? string.Join(' ', code.Scopes) : null,
            refreshToken);
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
