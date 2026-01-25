namespace OAuth2.Infrastructure.Persistence.Context;

public sealed class MongoCollectionProvider : IMongoCollectionProvider
{
    public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
    {
        throw new NotImplementedException();
    }
}
