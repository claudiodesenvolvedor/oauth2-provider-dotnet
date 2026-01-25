using System.Security.Cryptography;

namespace OAuth2.Infrastructure.Security;

internal static class RsaKeyLoader
{
    private const string PrivateKeyPemEnv = "JWT_PRIVATE_KEY_PEM";
    private const string PublicKeyPemEnv = "JWT_PUBLIC_KEY_PEM";
    private const string PrivateKeyPathEnv = "JWT_PRIVATE_KEY_PATH";
    private const string PublicKeyPathEnv = "JWT_PUBLIC_KEY_PATH";

    public static RSA LoadPrivateKey()
    {
        var pem = ResolvePem(PrivateKeyPemEnv, PrivateKeyPathEnv);
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem);
        return rsa;
    }

    public static RSA LoadPublicKey()
    {
        var pem = ResolvePem(PublicKeyPemEnv, PublicKeyPathEnv);
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem);
        return rsa;
    }

    private static string ResolvePem(string pemEnv, string pathEnv)
    {
        var pem = Environment.GetEnvironmentVariable(pemEnv);
        if (!string.IsNullOrWhiteSpace(pem))
        {
            return pem;
        }

        var path = Environment.GetEnvironmentVariable(pathEnv);
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        throw new InvalidOperationException(
            $"RSA key not found. Set {pemEnv} or {pathEnv}.");
    }
}
