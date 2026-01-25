namespace OAuth2.Application.Interfaces.Security;

public interface IPasswordHashService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
