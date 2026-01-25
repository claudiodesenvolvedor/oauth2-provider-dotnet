namespace OAuth2.Application.Interfaces.Security;

public interface IAccessTokenLifetimeProvider
{
    int GetAccessTokenMinutes();
}
