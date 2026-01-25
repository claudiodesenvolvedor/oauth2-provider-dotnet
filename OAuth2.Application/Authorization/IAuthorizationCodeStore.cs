namespace OAuth2.Application.Authorization;

public interface IAuthorizationCodeStore
{
    Task StoreAsync(AuthorizationCode code, CancellationToken cancellationToken);
    Task<AuthorizationCode?> GetAsync(string code, CancellationToken cancellationToken);
    Task InvalidateAsync(string code, CancellationToken cancellationToken);
}
