using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Domain.Entities;

namespace OAuth2.Infrastructure.Repositories;

public sealed class ClientRepository : IClientRepository
{
    public Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Client?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Client> CreateAsync(Client client, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Client?> UpdateAsync(Client client, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string clientId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
