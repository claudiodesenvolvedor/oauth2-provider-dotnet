using MongoDB.Driver;
using OAuth2.Application.Interfaces.Auth;
using OAuth2.Infrastructure.Persistence.Context;
using OAuth2.Infrastructure.Persistence.Documents;

namespace OAuth2.Infrastructure.Auth;

public sealed class MongoUserStore : IUserStore
{
    private const string CollectionName = "users";
    private readonly MongoDB.Driver.IMongoCollection<UserDocument> _users;

    public MongoUserStore(IMongoDbContext context)
    {
        _users = context.Collections.GetCollection<UserDocument>(CollectionName);
    }

    public async Task<UserRecord?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var document = await _users
            .Find(user => user.Email == email)
            .FirstOrDefaultAsync(cancellationToken);

        return document is null
            ? null
            : new UserRecord(
                document.Id,
                document.Email,
                document.PasswordHash,
                document.IsActive,
                document.CreatedAt);
    }

    public async Task<UserRecord> CreateAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Password is required.");
        }

        var document = new UserDocument
        {
            Id = Guid.NewGuid().ToString("N"),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _users.InsertOneAsync(document, cancellationToken: cancellationToken);

        return new UserRecord(
            document.Id,
            document.Email,
            document.PasswordHash,
            document.IsActive,
            document.CreatedAt);
    }
}
