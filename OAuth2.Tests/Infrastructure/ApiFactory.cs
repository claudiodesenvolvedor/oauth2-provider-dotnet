using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IClientStore));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IClientStore, InMemoryClientStore>();

            var codeDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(IAuthorizationCodeStore));

            if (codeDescriptor is not null)
            {
                services.Remove(codeDescriptor);
            }

            services.AddSingleton<IAuthorizationCodeStore, InMemoryAuthorizationCodeStore>();
        });
    }
}
