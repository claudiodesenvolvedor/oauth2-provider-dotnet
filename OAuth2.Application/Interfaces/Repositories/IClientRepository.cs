using OAuth2.Domain.Entities;

namespace OAuth2.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken);
    Task<Client?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken);
    Task<Client> CreateAsync(Client client, CancellationToken cancellationToken);
    Task<Client?> UpdateAsync(Client client, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string clientId, CancellationToken cancellationToken);
}
