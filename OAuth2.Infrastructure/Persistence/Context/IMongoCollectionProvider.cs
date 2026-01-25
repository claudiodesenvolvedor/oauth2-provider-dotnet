using MongoDB.Driver;

namespace OAuth2.Infrastructure.Persistence.Context;

public interface IMongoCollectionProvider
{
    MongoDB.Driver.IMongoCollection<TDocument> GetCollection<TDocument>(string name);
}
