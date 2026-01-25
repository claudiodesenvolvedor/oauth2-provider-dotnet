using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Security;
using OAuth2.Application.Interfaces.Security;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route(".well-known")]
public sealed class JwksController : ControllerBase
{
    private readonly IJwtKeyProvider _jwtKeyProvider;

    public JwksController(IJwtKeyProvider jwtKeyProvider)
    {
        _jwtKeyProvider = jwtKeyProvider;
    }

    [HttpGet("jwks.json")]
    [ProducesResponseType(typeof(JwksResponse), StatusCodes.Status200OK)]
    public Task<JwksResponse> GetAsync(CancellationToken cancellationToken)
    {
        return _jwtKeyProvider.GetJwksAsync(cancellationToken);
    }
}
