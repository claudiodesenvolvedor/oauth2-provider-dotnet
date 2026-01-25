using Microsoft.Extensions.Options;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure.Security;

public sealed class AccessTokenLifetimeProvider : IAccessTokenLifetimeProvider
{
    private readonly JwtSettings _jwtSettings;

    public AccessTokenLifetimeProvider(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public int GetAccessTokenMinutes()
    {
        return _jwtSettings.AccessTokenMinutes <= 0 ? 10 : _jwtSettings.AccessTokenMinutes;
    }
}
