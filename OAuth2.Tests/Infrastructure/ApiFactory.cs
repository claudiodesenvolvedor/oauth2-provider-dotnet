using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

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
    }
}
