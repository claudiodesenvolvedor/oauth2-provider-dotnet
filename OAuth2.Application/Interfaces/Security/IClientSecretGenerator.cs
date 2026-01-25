namespace OAuth2.Application.Interfaces.Security;

public interface IClientSecretGenerator
{
    string GenerateSecret();
}
