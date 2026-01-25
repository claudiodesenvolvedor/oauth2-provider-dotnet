using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public Task<LoginResponse> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(request, cancellationToken);
    }
}
