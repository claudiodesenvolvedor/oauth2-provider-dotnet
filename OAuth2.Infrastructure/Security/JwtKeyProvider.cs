using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using OAuth2.Application.DTOs.Security;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure.Security;

public sealed class JwtKeyProvider : IJwtKeyProvider
{
    private readonly JwtSettings _jwtSettings;

    public JwtKeyProvider(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public Task<JwksResponse> GetJwksAsync(CancellationToken cancellationToken)
    {
        using var rsa = RsaKeyLoader.LoadPublicKey();
        var parameters = rsa.ExportParameters(false);

        var keyId = _jwtSettings.KeyId;
        if (string.IsNullOrWhiteSpace(keyId))
        {
            throw new InvalidOperationException("Jwt:KeyId is required.");
        }

        var jwk = new JwksKey(
            "RSA",
            "sig",
            "RS256",
            keyId,
            Base64UrlEncode(parameters.Modulus ?? Array.Empty<byte>()),
            Base64UrlEncode(parameters.Exponent ?? Array.Empty<byte>()));

        return Task.FromResult(new JwksResponse(new[] { jwk }));
    }

    private static string Base64UrlEncode(byte[] data)
    {
        return Convert.ToBase64String(data)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
