namespace OAuth2.Application.Interfaces.Auth;

public interface IUserStore
{
    Task<bool> AnyAsync(CancellationToken cancellationToken);
    Task<UserRecord?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserRecord> CreateAsync(string email, string password, CancellationToken cancellationToken);
}
