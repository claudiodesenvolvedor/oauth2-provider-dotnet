using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.DTOs.Errors;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAdminAuthService _adminAuthService;

    public AuthController(IAdminAuthService adminAuthService)
    {
        _adminAuthService = adminAuthService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AdminLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public Task<AdminLoginResponse> LoginAsync(
        [FromBody] AdminLoginRequest request,
        CancellationToken cancellationToken)
    {
        return _adminAuthService.LoginAsync(request, cancellationToken);
    }
}
