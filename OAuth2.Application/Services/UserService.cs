using OAuth2.Application.DTOs.Users;
using OAuth2.Application.Interfaces.Repositories;
using OAuth2.Application.Interfaces.Services;
using OAuth2.Domain.Entities;

namespace OAuth2.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponse?> UpdateAsync(string id, UserUpdateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
