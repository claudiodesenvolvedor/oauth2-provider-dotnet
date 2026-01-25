using OAuth2.Application.DTOs.Auth;

namespace OAuth2.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<TokenResponse> GenerateTokenAsync(TokenRequest request, CancellationToken cancellationToken);
}
