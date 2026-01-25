using OAuth2.Application.DTOs.Security;
using OAuth2.Application.Interfaces.Security;

namespace OAuth2.Infrastructure.Security;

public sealed class JwtKeyProvider : IJwtKeyProvider
{
    private readonly IRsaKeyProvider _rsaKeyProvider;

    public JwtKeyProvider(IRsaKeyProvider rsaKeyProvider)
    {
        _rsaKeyProvider = rsaKeyProvider;
    }

    public Task<JwksResponse> GetJwksAsync(CancellationToken cancellationToken)
    {
        var parameters = _rsaKeyProvider.GetPublicKey().ExportParameters(false);
        var keyId = _rsaKeyProvider.GetKeyId();

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
