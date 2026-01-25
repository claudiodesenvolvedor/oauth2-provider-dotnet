using OAuth2.Domain.Entities;

namespace OAuth2.Application.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<Token> CreateAsync(Token token, CancellationToken cancellationToken);
    Task<Token?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
