namespace OAuth2.Infrastructure.Persistence.Context;

public interface IMongoCollectionProvider
{
    IMongoCollection<TDocument> GetCollection<TDocument>(string name);
}
