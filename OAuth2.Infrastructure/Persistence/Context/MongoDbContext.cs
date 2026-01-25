namespace OAuth2.Infrastructure.Persistence.Context;

public sealed class MongoDbContext : IMongoDbContext
{
    public MongoDbContext(IMongoCollectionProvider collections)
    {
        Collections = collections;
    }

    public IMongoCollectionProvider Collections { get; }
}
