using System.Security.Cryptography;
using OAuth2.Application.Authorization;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Application.Services;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private const int RefreshTokenDays = 30;
    private readonly IClientStore _clientStore;
    private readonly IRefreshTokenStore _refreshTokenStore;
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenLifetimeProvider _tokenLifetimeProvider;

    public RefreshTokenService(
        IClientStore clientStore,
        IRefreshTokenStore refreshTokenStore,
        ITokenService tokenService,
        IAccessTokenLifetimeProvider tokenLifetimeProvider)
    {
        _clientStore = clientStore;
        _refreshTokenStore = refreshTokenStore;
        _tokenService = tokenService;
        _tokenLifetimeProvider = tokenLifetimeProvider;
    }

    public async Task<string> CreateAsync(
        string clientId,
        string subject,
        IReadOnlyList<string> scopes,
        CancellationToken cancellationToken)
    {
        var token = GenerateRefreshToken();
        var expiresAt = DateTimeOffset.UtcNow.AddDays(RefreshTokenDays);

        var refreshToken = new RefreshToken(
            token,
            clientId,
            subject,
            scopes,
            expiresAt);

        await _refreshTokenStore.StoreAsync(refreshToken, cancellationToken);
        return token;
    }

    public async Task<RefreshTokenResponse> ExchangeAsync(
        RefreshTokenRequest request,
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

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new InvalidOperationException("Invalid refresh_token.");
        }

        var stored = await _refreshTokenStore.GetAsync(request.RefreshToken, cancellationToken);
        if (stored is null)
        {
            throw new InvalidOperationException("Invalid refresh_token.");
        }

        if (stored.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            await _refreshTokenStore.InvalidateAsync(request.RefreshToken, cancellationToken);
            throw new InvalidOperationException("Refresh token expired.");
        }

        if (!string.Equals(stored.ClientId, request.ClientId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid client.");
        }

        await _refreshTokenStore.InvalidateAsync(request.RefreshToken, cancellationToken);

        var newRefreshToken = await CreateAsync(
            stored.ClientId,
            stored.Subject,
            stored.Scopes,
            cancellationToken);

        var accessExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes());
        var accessToken = _tokenService.GenerateAccessToken(
            stored.Subject,
            stored.Scopes,
            accessExpiresAt);

        return new RefreshTokenResponse(
            accessToken,
            "Bearer",
            (int)TimeSpan.FromMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes()).TotalSeconds,
            stored.Scopes.Count > 0 ? string.Join(' ', stored.Scopes) : null,
            newRefreshToken);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Base64UrlEncode(bytes);
    }

    private static string Base64UrlEncode(byte[] data)
    {
        return Convert.ToBase64String(data)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
