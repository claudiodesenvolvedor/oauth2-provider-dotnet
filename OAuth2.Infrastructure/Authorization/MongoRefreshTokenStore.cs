using MongoDB.Driver;
using OAuth2.Application.Authorization;
using OAuth2.Infrastructure.Persistence.Context;
using OAuth2.Infrastructure.Persistence.Documents;
using OAuth2.Infrastructure.Persistence.Mappers;

namespace OAuth2.Infrastructure.Authorization;

public sealed class MongoRefreshTokenStore : IRefreshTokenStore
{
    private const string CollectionName = "refresh_tokens";
    private readonly MongoDB.Driver.IMongoCollection<RefreshTokenDocument> _collection;

    public MongoRefreshTokenStore(IMongoDbContext context)
    {
        _collection = context.Collections.GetCollection<RefreshTokenDocument>(CollectionName);
        EnsureIndexes();
    }

    public async Task StoreAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        var document = RefreshTokenMapper.ToDocument(token);
        var filter = Builders<RefreshTokenDocument>.Filter.Eq(x => x.Token, token.Token);
        await _collection.ReplaceOneAsync(
            filter,
            document,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }

    public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken)
    {
        var document = await _collection
            .Find(x => x.Token == token)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null ? null : RefreshTokenMapper.ToDomain(document);
    }

    public Task InvalidateAsync(string token, CancellationToken cancellationToken)
    {
        return _collection.DeleteOneAsync(x => x.Token == token, cancellationToken);
    }

    private void EnsureIndexes()
    {
        var indexKeys = Builders<RefreshTokenDocument>.IndexKeys.Ascending(x => x.ExpiresAt);
        var indexModel = new CreateIndexModel<RefreshTokenDocument>(
            indexKeys,
            new CreateIndexOptions { ExpireAfter = TimeSpan.Zero });

        _collection.Indexes.CreateOne(indexModel);
    }
}
