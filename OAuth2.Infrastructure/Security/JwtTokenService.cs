using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure.Security;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateAccessToken(
        string subject,
        IReadOnlyList<string> scopes,
        DateTimeOffset expiresAt)
    {
        var keyId = _jwtSettings.KeyId;
        if (string.IsNullOrWhiteSpace(keyId))
        {
            throw new InvalidOperationException("Jwt:KeyId is required.");
        }

        using var rsa = RsaKeyLoader.LoadPrivateKey();
        var signingKey = new RsaSecurityKey(rsa) { KeyId = keyId };

        var now = DateTimeOffset.UtcNow;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        if (scopes.Count > 0)
        {
            claims.Add(new Claim("scope", string.Join(' ', scopes)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt.UtcDateTime,
            NotBefore = now.UtcDateTime,
            IssuedAt = now.UtcDateTime,
            SigningCredentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.RsaSha256)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
