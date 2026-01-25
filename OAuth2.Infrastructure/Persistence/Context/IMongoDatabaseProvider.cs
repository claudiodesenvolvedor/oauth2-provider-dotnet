using MongoDB.Driver;

namespace OAuth2.Infrastructure.Persistence.Context;

public interface IMongoDatabaseProvider
{
    IMongoDatabase Database { get; }
}
