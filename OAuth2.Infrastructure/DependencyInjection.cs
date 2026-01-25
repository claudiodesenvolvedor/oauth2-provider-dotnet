using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Auth;
using OAuth2.Infrastructure.Authorization;
using OAuth2.Infrastructure.Persistence.Context;
using OAuth2.Infrastructure.Repositories;
using OAuth2.Infrastructure.Security;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

        services.AddScoped<IClientStore, MongoClientStore>();

        services.AddSingleton<IMongoDatabaseProvider, MongoDatabaseProvider>();
        services.AddScoped<IMongoDbContext, MongoDbContext>();
        services.AddScoped<IMongoCollectionProvider, MongoCollectionProvider>();

        services.AddScoped<IAuthorizationCodeStore, MongoAuthorizationCodeStore>();
        services.AddScoped<IRefreshTokenStore, MongoRefreshTokenStore>();
        services.AddScoped<IScopeProvider, ScopeProvider>();
        services.AddScoped<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        services.AddSingleton<IRsaKeyProvider, RsaKeyProvider>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddSingleton<IJwtKeyProvider, JwtKeyProvider>();
        services.AddScoped<IClientSecretGenerator, ClientSecretGenerator>();
        services.AddScoped<IUserPasswordPolicy, UserPasswordPolicy>();
        services.AddScoped<IClientIdGenerator, ClientIdGenerator>();
        services.AddScoped<IAccessTokenLifetimeProvider, AccessTokenLifetimeProvider>();

        return services;
    }
}
