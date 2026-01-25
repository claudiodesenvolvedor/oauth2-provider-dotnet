using OAuth2.Application.DTOs.Auth;

namespace OAuth2.Application.Interfaces.Services;

public interface IClientCredentialsService
{
    Task<ClientCredentialsTokenResponse> IssueTokenAsync(
        ClientCredentialsTokenRequest request,
        CancellationToken cancellationToken);
}
