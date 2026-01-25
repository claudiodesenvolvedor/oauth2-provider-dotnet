namespace OAuth2.Application.Interfaces.Security;

public interface IUserPasswordPolicy
{
    bool IsValid(string password, out IReadOnlyList<string> errors);
}
