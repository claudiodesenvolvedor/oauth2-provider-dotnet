using OAuth2.Application.Interfaces.Security;

namespace OAuth2.Infrastructure.Security;

public sealed class UserPasswordPolicy : IUserPasswordPolicy
{
    public bool IsValid(string password, out IReadOnlyList<string> errors)
    {
        throw new NotImplementedException();
    }
}
