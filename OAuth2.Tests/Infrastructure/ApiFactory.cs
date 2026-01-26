using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Auth;
using OAuth2.Infrastructure.Authorization;

namespace OAuth2.Tests.Infrastructure;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _keyId = "test-kid";

    public ApiFactory()
    {
        using var rsa = RSA.Create(2048);
        var privatePem = new string(
            PemEncoding.Write("PRIVATE KEY", rsa.ExportPkcs8PrivateKey()));
        var publicPem = new string(
            PemEncoding.Write("PUBLIC KEY", rsa.ExportSubjectPublicKeyInfo()));

        Environment.SetEnvironmentVariable("JWT_PRIVATE_KEY_PEM", privatePem);
        Environment.SetEnvironmentVariable("JWT_PUBLIC_KEY_PEM", publicPem);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "https://test-issuer",
                ["Jwt:Audience"] = "https://test-audience",
                ["Jwt:AccessTokenMinutes"] = "5",
                ["Jwt:KeyId"] = _keyId
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IClientStore));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IClientStore, InMemoryClientStore>();

            var userDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IUserStore));

            if (userDescriptor is not null)
            {
                services.Remove(userDescriptor);
            }

            var userStore = new InMemoryUserStore();
            userStore.CreateAsync("admin@local", "Admin123!", CancellationToken.None)
                .GetAwaiter()
                .GetResult();
            services.AddSingleton<IUserStore>(userStore);

            var codeDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IAuthorizationCodeStore));

            if (codeDescriptor is not null)
            {
                services.Remove(codeDescriptor);
            }

            services.AddSingleton<IAuthorizationCodeStore, InMemoryAuthorizationCodeStore>();

            var refreshDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IRefreshTokenStore));

            if (refreshDescriptor is not null)
            {
                services.Remove(refreshDescriptor);
            }

            services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();
        });
    }
}
