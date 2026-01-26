using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Application.Services;

public sealed class AdminAuthService : IAdminAuthService
{
    private readonly IUserStore _userStore;
    private readonly ITokenService _tokenService;
    private readonly IAccessTokenLifetimeProvider _tokenLifetimeProvider;

    public AdminAuthService(
        IUserStore userStore,
        ITokenService tokenService,
        IAccessTokenLifetimeProvider tokenLifetimeProvider)
    {
        _userStore = userStore;
        _tokenService = tokenService;
        _tokenLifetimeProvider = tokenLifetimeProvider;
    }

    public async Task<AdminLoginResponse> LoginAsync(
        AdminLoginRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException("Password is required.");
        }

        var user = await _userStore.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User is inactive.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes());
        var accessToken = _tokenService.GenerateAccessToken(
            user.Id,
            new[] { "admin" },
            expiresAt);

        return new AdminLoginResponse(
            accessToken,
            (int)TimeSpan.FromMinutes(_tokenLifetimeProvider.GetAccessTokenMinutes()).TotalSeconds);
    }
}
