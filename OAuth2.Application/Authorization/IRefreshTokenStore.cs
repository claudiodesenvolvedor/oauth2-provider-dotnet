namespace OAuth2.Application.Authorization;

public interface IRefreshTokenStore
{
    Task StoreAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken);
    Task InvalidateAsync(string token, CancellationToken cancellationToken);
}
