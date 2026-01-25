using MongoDB.Driver;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Infrastructure.Persistence.Context;
using OAuth2.Infrastructure.Persistence.Documents;

namespace OAuth2.Infrastructure.Auth;

public sealed class MongoClientStore : IClientStore
{
    private const string CollectionName = "clients";
    private readonly MongoDB.Driver.IMongoCollection<ClientDocument> _clients;

    public MongoClientStore(IMongoDbContext context)
    {
        _clients = context.Collections.GetCollection<ClientDocument>(CollectionName);
    }

    public async Task<ClientRecord?> GetByClientIdAsync(
        string clientId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return null;
        }

        var document = await _clients
            .Find(doc => doc.ClientId == clientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (document is null)
        {
            return null;
        }

        var scopes = document.AllowedScopes ?? Array.Empty<string>();
        return new ClientRecord(document.ClientId, document.ClientSecretHash, scopes);
    }
}
