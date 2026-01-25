using OAuth2.Application.Interfaces.Security;

namespace OAuth2.Infrastructure.Security;

public sealed class PasswordHashService : IPasswordHashService
{
    public string HashPassword(string password)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        throw new NotImplementedException();
    }
}
