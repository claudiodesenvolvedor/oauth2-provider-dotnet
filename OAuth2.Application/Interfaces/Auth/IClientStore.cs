namespace OAuth2.Application.Interfaces.Auth;

public interface IClientStore
{
    Task<ClientRecord?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken);
}
