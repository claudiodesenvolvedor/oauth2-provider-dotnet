using OAuth2.Application.DTOs.Clients;

namespace OAuth2.Application.Interfaces.Services;

public interface IClientService
{
    Task<IReadOnlyList<ClientResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<ClientResponse?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken);
    Task<ClientResponse> CreateAsync(ClientCreateRequest request, CancellationToken cancellationToken);
    Task<ClientResponse?> UpdateAsync(string clientId, ClientUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string clientId, CancellationToken cancellationToken);
}
