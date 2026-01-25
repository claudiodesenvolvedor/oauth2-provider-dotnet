using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Domain.Entities;

namespace OAuth2.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
