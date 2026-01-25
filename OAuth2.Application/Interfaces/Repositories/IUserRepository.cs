using OAuth2.Domain.Entities;

namespace OAuth2.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken);
    Task<User?> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}
