using OAuth2.Application.DTOs.Users;

namespace OAuth2.Application.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<UserResponse> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken);
    Task<UserResponse?> UpdateAsync(string id, UserUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}
