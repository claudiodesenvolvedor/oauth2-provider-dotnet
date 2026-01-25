using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Domain.Entities;

namespace OAuth2.Infrastructure.Repositories;

public sealed class TokenRepository : ITokenRepository
{
    public Task<Token> CreateAsync(Token token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Token?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
