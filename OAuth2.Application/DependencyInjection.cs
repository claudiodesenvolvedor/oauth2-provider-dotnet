using Microsoft.Extensions.DependencyInjection;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Application.Services;

namespace OAuth2.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IAuthorizationCodeService, AuthorizationCodeService>();
        services.AddScoped<IClientCredentialsService, ClientCredentialsService>();

        return services;
    }
}
