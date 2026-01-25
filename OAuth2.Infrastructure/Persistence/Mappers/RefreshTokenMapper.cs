using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Persistence.Documents;

namespace OAuth2.Infrastructure.Persistence.Mappers;

public static class RefreshTokenMapper
{
    public static RefreshTokenDocument ToDocument(RefreshToken token)
    {
        return new RefreshTokenDocument
        {
            Token = token.Token,
            ClientId = token.ClientId,
            Subject = token.Subject,
            Scopes = token.Scopes,
            ExpiresAt = token.ExpiresAt
        };
    }

    public static RefreshToken ToDomain(RefreshTokenDocument document)
    {
        return new RefreshToken(
            document.Token,
            document.ClientId,
            document.Subject,
            document.Scopes,
            document.ExpiresAt);
    }
}
