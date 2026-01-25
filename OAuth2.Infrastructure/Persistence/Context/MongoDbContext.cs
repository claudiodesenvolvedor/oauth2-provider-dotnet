namespace OAuth2.Infrastructure.Persistence.Context;

public sealed class MongoDbContext : IMongoDbContext
{
    public IMongoCollectionProvider Collections => throw new NotImplementedException();
}
