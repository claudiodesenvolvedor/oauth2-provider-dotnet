using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Persistence.Documents;

namespace OAuth2.Infrastructure.Persistence.Mappers;

public static class AuthorizationCodeMapper
{
    public static AuthorizationCodeDocument ToDocument(AuthorizationCode code)
    {
        return new AuthorizationCodeDocument
        {
            Code = code.Code,
            ClientId = code.ClientId,
            Subject = code.Subject,
            RedirectUri = code.RedirectUri,
            Scopes = code.Scopes,
            ExpiresAt = code.ExpiresAt
        };
    }

    public static AuthorizationCode ToDomain(AuthorizationCodeDocument document)
    {
        return new AuthorizationCode(
            document.Code,
            document.ClientId,
            document.Subject,
            document.Scopes,
            document.RedirectUri,
            document.ExpiresAt);
    }
}
