using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Users;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public Task<IActionResult> CreateAsync(
        [FromBody] UserCreateRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateAsync(
        string id,
        [FromBody] UserUpdateRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }
}
