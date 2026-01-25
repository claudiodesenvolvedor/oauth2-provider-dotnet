using MongoDB.Driver;
using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Persistence.Context;
using OAuth2.Infrastructure.Persistence.Documents;
using OAuth2.Infrastructure.Persistence.Mappers;

namespace OAuth2.Infrastructure.Authorization;

public sealed class MongoAuthorizationCodeStore : IAuthorizationCodeStore
{
    private const string CollectionName = "authorization_codes";
    private readonly MongoDB.Driver.IMongoCollection<AuthorizationCodeDocument> _collection;

    public MongoAuthorizationCodeStore(IMongoDbContext context)
    {
        _collection = context.Collections.GetCollection<AuthorizationCodeDocument>(CollectionName);
        EnsureIndexes();
    }

    public async Task StoreAsync(AuthorizationCode code, CancellationToken cancellationToken)
    {
        var document = AuthorizationCodeMapper.ToDocument(code);
        var filter = Builders<AuthorizationCodeDocument>.Filter.Eq(x => x.Code, code.Code);
        await _collection.ReplaceOneAsync(
            filter,
            document,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }

    public async Task<AuthorizationCode?> GetAsync(string code, CancellationToken cancellationToken)
    {
        var document = await _collection
            .Find(x => x.Code == code)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null ? null : AuthorizationCodeMapper.ToDomain(document);
    }

    public Task InvalidateAsync(string code, CancellationToken cancellationToken)
    {
        return _collection.DeleteOneAsync(x => x.Code == code, cancellationToken);
    }

    private void EnsureIndexes()
    {
        var indexKeys = Builders<AuthorizationCodeDocument>.IndexKeys.Ascending(x => x.ExpiresAt);
        var indexModel = new CreateIndexModel<AuthorizationCodeDocument>(
            indexKeys,
            new CreateIndexOptions { ExpireAfter = TimeSpan.Zero });

        _collection.Indexes.CreateOne(indexModel);
    }
}
