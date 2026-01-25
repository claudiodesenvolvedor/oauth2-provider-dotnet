using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure.Security;

public sealed class RsaKeyProvider : IRsaKeyProvider
{
    private readonly RSA _privateKey;
    private readonly RSA _publicKey;
    private readonly string _keyId;

    public RsaKeyProvider(IOptions<JwtSettings> jwtOptions)
    {
        var settings = jwtOptions.Value;
        if (string.IsNullOrWhiteSpace(settings.KeyId))
        {
            throw new InvalidOperationException("Jwt:KeyId is required.");
        }

        _keyId = settings.KeyId;
        _privateKey = RsaKeyLoader.LoadPrivateKey();
        _publicKey = RsaKeyLoader.LoadPublicKey();
    }

    public RSA GetPrivateKey()
    {
        return _privateKey;
    }

    public RSA GetPublicKey()
    {
        return _publicKey;
    }

    public string GetKeyId()
    {
        return _keyId;
    }
}
