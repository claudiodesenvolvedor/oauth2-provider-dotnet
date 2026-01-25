using MongoDB.Driver;

namespace OAuth2.Infrastructure.Persistence.Context;

public sealed class MongoCollectionProvider : IMongoCollectionProvider
{
    private readonly IMongoDatabaseProvider _databaseProvider;

    public MongoCollectionProvider(IMongoDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public MongoDB.Driver.IMongoCollection<TDocument> GetCollection<TDocument>(string name)
    {
        return _databaseProvider.Database.GetCollection<TDocument>(name);
    }
}
