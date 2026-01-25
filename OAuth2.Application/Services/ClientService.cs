using OAuth2.Application.DTOs.Clients;
using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Domain.Entities;

namespace OAuth2.Application.Services;

public sealed class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public Task<IReadOnlyList<ClientResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ClientResponse?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ClientResponse> CreateAsync(ClientCreateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ClientResponse?> UpdateAsync(string clientId, ClientUpdateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string clientId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
