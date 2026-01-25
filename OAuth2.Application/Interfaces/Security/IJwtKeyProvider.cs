using OAuth2.Application.DTOs.Security;

namespace OAuth2.Application.Interfaces.Security;

public interface IJwtKeyProvider
{
    Task<JwksResponse> GetJwksAsync(CancellationToken cancellationToken);
}
