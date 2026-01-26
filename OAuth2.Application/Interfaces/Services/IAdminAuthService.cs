using OAuth2.Application.DTOs.Auth;

namespace OAuth2.Application.Interfaces.Services;

public interface IAdminAuthService
{
    Task<AdminLoginResponse> LoginAsync(
        AdminLoginRequest request,
        CancellationToken cancellationToken);
}
