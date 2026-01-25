using System.Security.Cryptography;

namespace OAuth2.Application.Interfaces.Security;

public interface IRsaKeyProvider
{
    RSA GetPrivateKey();
    RSA GetPublicKey();
    string GetKeyId();
}
