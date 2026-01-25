using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OAuth2.Application.DTOs.Security;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Api;

public sealed class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
{
    private readonly IOptions<JwtSettings> _jwtOptions;
    private readonly IJwtKeyProvider _jwtKeyProvider;

    public JwtBearerOptionsSetup(
        IOptions<JwtSettings> jwtOptions,
        IJwtKeyProvider jwtKeyProvider)
    {
        _jwtOptions = jwtOptions;
        _jwtKeyProvider = jwtKeyProvider;
    }

    public void Configure(JwtBearerOptions options)
    {
        var jwtSettings = _jwtOptions.Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                var jwks = _jwtKeyProvider.GetJwksAsync(CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();

                return jwks.Keys
                    .Where(key => string.IsNullOrWhiteSpace(kid) || key.Kid == kid)
                    .Select(BuildRsaSecurityKey)
                    .ToList();
            }
        };
    }

    private static SecurityKey BuildRsaSecurityKey(JwksKey key)
    {
        var parameters = new RSAParameters
        {
            Modulus = Base64UrlDecode(key.N),
            Exponent = Base64UrlDecode(key.E)
        };

        return new RsaSecurityKey(parameters) { KeyId = key.Kid };
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var padding = value.Length % 4;
        if (padding > 0)
        {
            value = value + new string('=', 4 - padding);
        }

        value = value.Replace('-', '+').Replace('_', '/');
        return Convert.FromBase64String(value);
    }
}
