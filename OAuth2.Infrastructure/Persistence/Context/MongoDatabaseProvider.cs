using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OAuth2.Infrastructure.Settings;

namespace OAuth2.Infrastructure.Persistence.Context;

internal sealed class MongoDatabaseProvider : IMongoDatabaseProvider
{
    private readonly IMongoDatabase _database;
    private readonly MongoClient _client;

    public MongoDatabaseProvider(IOptions<MongoDbSettings> settings)
    {
        var config = settings.Value;
        if (string.IsNullOrWhiteSpace(config.ConnectionString))
        {
            throw new InvalidOperationException("MongoDb:ConnectionString is required.");
        }

        if (string.IsNullOrWhiteSpace(config.DatabaseName))
        {
            throw new InvalidOperationException("MongoDb:DatabaseName is required.");
        }

        _client = new MongoClient(config.ConnectionString);
        _database = _client.GetDatabase(config.DatabaseName);
    }

    public IMongoDatabase Database => _database;
}
