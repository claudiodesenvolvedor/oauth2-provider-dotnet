using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Domain.Entities;

namespace OAuth2.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ITokenRepository _tokenRepository;

    public AuthService(
        IUserRepository userRepository,
        IClientRepository clientRepository,
        ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        _tokenRepository = tokenRepository;
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TokenResponse> GenerateTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
