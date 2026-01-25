namespace OAuth2.Infrastructure.Persistence.Context;

public interface IMongoDbContext
{
    IMongoCollectionProvider Collections { get; }
}
